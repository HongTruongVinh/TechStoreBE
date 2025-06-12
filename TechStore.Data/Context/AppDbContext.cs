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
    public class AppDbContext : DbContext
    {
        public DbSet<Sequence> Sequences => Set<Sequence>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<InvalidToken> InvalidTokens => Set<InvalidToken>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<QRCode> QRCodes => Set<QRCode>();
        public DbSet<Report> Reports => Set<Report>();
        public DbSet<Shipper> Shippers => Set<Shipper>();
        public DbSet<ShippingDetail> ShippingDetails => Set<ShippingDetail>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Voucher> Vouchers => Set<Voucher>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Cấu hình DB

            // Cấu hình Brand
            modelBuilder.Entity<Brand>(entity =>
            {
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

            // Cấu hình Category
            modelBuilder.Entity<Category>(entity =>
            {
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

            // Cấu hình Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(i => i.Order)
                .HasForeignKey<Payment>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(i => i.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShippingDetail>()
                .HasOne(p => p.Shipper)
                .WithMany()
                .HasForeignKey(p => p.ShipperId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShippingDetail>()
                .HasOne(s => s.Order)
                .WithOne(o => o.ShippingDetail)
                .HasForeignKey<ShippingDetail>(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingDetail)
                .WithOne(s => s.Order)
                .HasForeignKey<ShippingDetail>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Orders)
            //    .WithOne(o => o.Customer)
            //    .HasForeignKey(o => o.CustomerId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.CartItems)
            //    .WithOne(o => o.User)
            //    .HasForeignKey(o => o.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            #endregion


        }
    }
}
