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
        public ICartItemRepository CartItems { get; }
        public ICommentRepository Comments { get; }
        public IInvoiceRepository Invoices { get; }
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public IPaymentRepository Payments { get; }
        public IQRCodeRepository QRCodes { get; }
        public IReportRepository Reports { get; }
        public IShipperRepository Shippers { get; }
        public IShippingDetailRepository ShippingDetails { get; }
        public IUserRepository Users { get; }
        public IVoucherRepository Vouchers { get; }
        public IInvalidTokenRepository InvalidTokens { get; }
        public ISequenceRepository Sequences { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Brands = new BrandRepository(_context);
            Categories = new CategoryRepository(_context);
            Products = new ProductRepository(_context);
            CartItems = new CartItemRepository(_context);
            Comments = new CommentRepository(_context);
            Invoices = new InvoiceRepository(_context);
            Orders = new OrderRepository(_context);
            OrderItems = new OrderItemRepository(_context);
            Payments = new PaymentRepository(_context);
            QRCodes = new QRCodeRepository(_context);
            Reports = new ReportRepository(_context);
            Shippers = new ShipperRepository(_context);
            ShippingDetails = new ShippingDetailRepository(_context);
            Users = new UserRepository(_context);
            Vouchers = new VoucherRepository(_context);
            InvalidTokens = new InvalidTokenRepository(_context);
            Sequences = new SequenceRepository(_context);
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
