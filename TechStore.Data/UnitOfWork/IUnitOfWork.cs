using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Repositories.Implementations;
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
        IPaymentTransactionRepository PaymentTransactions { get; }
        IPaymentSnapshotRepository PaymentSnapshots { get; }
        IPaymentSnapshotItemRepository PaymentSnapshotItems { get; }
        IReportRepository Reports { get; }
        IShipperRepository Shippers { get; }
        IShippingDetailRepository ShippingDetails { get; }
        IStockReservationRepository StockReservations { get; }
        IUserRepository Users { get; }
        IVoucherRepository Vouchers { get; }
        IVoucherUsageRepository VoucherUsages { get; }
        ISearchKeywordRepository SearchKeywords { get; }
        ISequenceRepository Sequences { get; }
        IInvalidTokenRepository InvalidTokens { get; }
        IIdempotencyKeyRepository IdempotencyKeys { get; }
        ISystemConfigRepository SystemConfigs { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
