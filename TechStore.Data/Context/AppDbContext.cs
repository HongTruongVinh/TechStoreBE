using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Context
{
    // Add-Migration InitialCreate
    // Update-Database

    // Drop-Database
    public class AppDbContext : DbContext
    {
        public DbSet<Sequence> Sequences => Set<Sequence>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductVariant> ProductVariants  => Set<ProductVariant>();
        public DbSet<ProductVariantOption> ProductVariantOptions => Set<ProductVariantOption>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<InvalidToken> InvalidTokens => Set<InvalidToken>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentSnapshot> PaymentSnapshots => Set<PaymentSnapshot>();
        public DbSet<PaymentSnapshotItem> PaymentSnapshotItems => Set<PaymentSnapshotItem>();
        public DbSet<QRCode> QRCodes => Set<QRCode>();
        public DbSet<Report> Reports => Set<Report>();
        public DbSet<Shipper> Shippers => Set<Shipper>();
        public DbSet<ShippingDetail> ShippingDetails => Set<ShippingDetail>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Voucher> Vouchers => Set<Voucher>();
        public DbSet<SearchKeyword> SearchKeywords => Set<SearchKeyword>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Cấu hình DB

            // Cấu hình Brand
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .HasMaxLength(500); // Optional, cho phép null

                entity.Property(c => c.Slug)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(c => c.IconImageUrl)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasIndex(c => c.Slug).IsUnique(); // nếu muốn slug là duy nhất
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .HasMaxLength(500); // Optional, cho phép null

                entity.Property(c => c.Slug)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(c => c.IconImageUrl)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasIndex(c => c.Slug).IsUnique(); // slug là duy nhất
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(i => i.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(o => o.Payments)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.ShippingDetail)
                .WithOne(s => s.Order)
                .HasForeignKey<ShippingDetail>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.ProductVariantOption)
                .WithMany()
                .HasForeignKey(p => p.ProductVariantOptionId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<PaymentSnapshot>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasMany(p => p.Items)
                .WithOne(ps => ps.PaymentSnapshot)
                .HasForeignKey(p => p.PaymentSnapshotId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<PaymentSnapshotItem>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(p => p.PaymentSnapshot)
                .WithMany(ps => ps.Items)
                .HasForeignKey(p => p.PaymentSnapshotId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

                entity.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.Variants)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

                entity.HasOne(v => v.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(v => v.Options)
                .WithOne(op => op.ProductVariant)
                .HasForeignKey(op => op.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductVariantOption>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

                entity.HasOne(op => op.ProductVariant)
                .WithMany(pv => pv.Options)
                .HasForeignKey(v => v.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<QRCode>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

            });

            modelBuilder.Entity<ShippingDetail>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

                entity.HasOne(p => p.Shipper)
                .WithMany()
                .HasForeignKey(p => p.ShipperId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Order)
                .WithOne(o => o.ShippingDetail)
                .HasForeignKey<ShippingDetail>(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

            });

            modelBuilder.Entity<SearchKeyword>(entity =>
            {
                entity.HasIndex(p => p.PublicId).IsUnique();

            });

            #endregion


        }
    }
}
