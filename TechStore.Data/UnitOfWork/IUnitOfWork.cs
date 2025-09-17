using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepository Brands { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IProductVariantRepository ProductVariants { get; }
        IProductVariantOptionRepository ProductVariantOptions { get; }
        ICartItemRepository CartItems { get; }
        ICommentRepository Comments { get; }
        IInvoiceRepository Invoices { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        IPaymentRepository Payments { get; }
        IQRCodeRepository QRCodes { get; }
        IReportRepository Reports { get; }
        IShipperRepository Shippers { get; }
        IShippingDetailRepository ShippingDetails { get; }
        IUserRepository Users { get; }
        IVoucherRepository Vouchers { get; }
        ISequenceRepository Sequences { get; }
        IInvalidTokenRepository InvalidTokens { get; }

        Task<int> CommitAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
