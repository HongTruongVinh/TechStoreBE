using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.User;
using TechStore.Model.DTOs.Voucher;

namespace TechStore.Service.Mappers
{
    public static class VoucherMappers
    {
        public static VoucherResponseModel ToVoucherResponseModel(this Voucher voucher)
        {
            return new VoucherResponseModel
            {
                Id = voucher.PublicId,
                Code = voucher.Code,
                Description = voucher.Description,
                DiscountType = voucher.DiscountType,
                DiscountValue = voucher.DiscountValue,
                MaxDiscountAmount = voucher.MaxDiscountAmount,
                MinOrderPrice = voucher.MinOrderPrice,
                UsageLimit = voucher.UsageLimit,
                Available = voucher.Available,
                StartDate = voucher.StartDate,
                EndDate = voucher.EndDate,
                Status = voucher.Status
            };
        }
    }
}
