using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Context;
using TechStore.Data.Repositories.Implementations;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IBrandRepository Brands { get; }
        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }
        public IProductVariantRepository ProductVariants { get; }
        public IProductVariantOptionRepository ProductVariantOptions { get; }
        public ICartItemRepository CartItems { get; }
        public ICommentRepository Comments { get; }
        public IInvoiceRepository Invoices { get; }
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public IPaymentRepository Payments { get; }
        public IPaymentTransactionRepository PaymentTransactions { get; }
        public IPaymentSnapshotRepository PaymentSnapshots { get; }
        public IPaymentSnapshotItemRepository PaymentSnapshotItems { get; }
        public IVoucherUsageRepository QRCodes { get; }
        public IReportRepository Reports { get; }
        public IShipperRepository Shippers { get; }
        public IShippingDetailRepository ShippingDetails { get; }
        public IStockReservationRepository StockReservations { get; }
        public IUserRepository Users { get; }
        public IVoucherRepository Vouchers { get; }
        public IVoucherUsageRepository VoucherUsages { get; }
        public ISearchKeywordRepository SearchKeywords { get; }
        public IInvalidTokenRepository InvalidTokens { get; }
        public ISequenceRepository Sequences { get; }
        public IIdempotencyKeyRepository IdempotencyKeys { get; }
        public ISystemConfigRepository SystemConfigs { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Brands = new BrandRepository(_context);
            Categories = new CategoryRepository(_context);
            Products = new ProductRepository(_context);
            ProductVariants = new ProductVariantRepository(_context);
            ProductVariantOptions = new ProductVariantOptionRepository(_context);
            CartItems = new CartItemRepository(_context);
            Comments = new CommentRepository(_context);
            Invoices = new InvoiceRepository(_context);
            Orders = new OrderRepository(_context);
            OrderItems = new OrderItemRepository(_context);
            Payments = new PaymentRepository(_context);
            PaymentTransactions = new PaymentTransactionRepository(_context);
            PaymentSnapshots = new PaymentSnapshotRepository(_context);
            PaymentSnapshotItems = new PaymentSnapshotItemRepository(_context);
            QRCodes = new VoucherUsageRepository(_context);
            Reports = new ReportRepository(_context);
            Shippers = new ShipperRepository(_context);
            ShippingDetails = new ShippingDetailRepository(_context);
            StockReservations = new StockReservationRepository(_context);
            Users = new UserRepository(_context);
            Vouchers = new VoucherRepository(_context);
            VoucherUsages = new VoucherUsageRepository(_context);
            SearchKeywords = new SearchKeywordRepository(_context);
            InvalidTokens = new InvalidTokenRepository(_context);
            Sequences = new SequenceRepository(_context);
            IdempotencyKeys = new IdempotencyKeyRepository(_context);
            SystemConfigs = new SystemConfigRepository(_context);
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public void Dispose() => _context.Dispose();
    }
}
