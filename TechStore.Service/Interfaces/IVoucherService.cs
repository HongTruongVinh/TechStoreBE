using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Voucher;

namespace TechStore.Service.Interfaces
{
    public interface IVoucherService
    {
        Task<ServiceResult<VoucherResponseModel>> CheckVoucherAsync(string userId, string voucherCode, List<OrderItemCreateModel> products);
        Task<ServiceResult<List<VoucherResponseModel>>> GetVouchersAsync(string? userId);
    }
}
