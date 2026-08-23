using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Extensions;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;
using TechStore.Model.DTOs.Voucher;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _uow;

        public VoucherService(IUnitOfWork uow
            )
        {
            _uow = uow;
        }

        public async Task<ServiceResult<VoucherResponseModel>> CheckVoucherAsync(string userId, string voucherCode, List<OrderItemCreateModel> product)
        {
            var customer = await _uow.Users.TableNoTracking.Where(u => u.PublicId == userId).FirstOrDefaultAsync();

            if (customer == null)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.NotFound, Messenger.NotFoundUser);
            }

            var voucher = await _uow.Vouchers.TableNoTracking.Where(v => v.Code == voucherCode).FirstOrDefaultAsync();

            if (voucher == null)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.NotFound, VoucherMessenger.VoucherNotFound);
            }

            if (voucher.EndDate < DateTime.UtcNow)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
            }

            if (voucher.StartDate > DateTime.UtcNow)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
            }

            if (voucher.Status != EVoucherStatus.Active)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
            }

            if (voucher.Available <= 0)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherUsageExceeded);
            }

            var usageCount = await _uow.VoucherUsages.CountAsync(x => x.UserId == customer.Id && x.VoucherId == voucher.Id);

            if (usageCount >= voucher.UsageLimit)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherUsageExceeded);
            }

            decimal totalPrice = 0;
            foreach (var item in product)
            {
                var pVO = await _uow.ProductVariantOptions.GetOrderItemDetailAsync(item.ProductVariantOptionId);

                if (pVO == null)
                {
                    return ServiceResult<VoucherResponseModel>.Fail(EErrorType.NotFound, Messenger.NoExitData + " " + item.ProductVariantOptionId);
                }

                if (pVO.Stock < item.Quantity)
                {
                    return ServiceResult<VoucherResponseModel>.Fail(EErrorType.BadRequest, Messenger.NoExitData + " " + item.ProductVariantOptionId);
                }

                decimal itemTotal = item.Quantity * pVO.Price;
                totalPrice += itemTotal;
            }

            if(totalPrice < voucher.MinOrderPrice)
            {
                return ServiceResult<VoucherResponseModel>.Fail(EErrorType.BadRequest, VoucherMessenger.MinOrderPriceNotMet);
            }

            var model = voucher.ToVoucherResponseModel();

            return ServiceResult<VoucherResponseModel>.Success(model);
        }

        public async Task<ServiceResult<List<VoucherResponseModel>>> GetVouchersAsync(string? userId)
        {
            var vouchers = await _uow.Vouchers.TableNoTracking.Where(v => v.Status == EVoucherStatus.Active).ToListAsync();

            var models = vouchers.Select(v => v.ToVoucherResponseModel()).ToList();

            return ServiceResult<List<VoucherResponseModel>>.Success(models);
        }
    }
}
