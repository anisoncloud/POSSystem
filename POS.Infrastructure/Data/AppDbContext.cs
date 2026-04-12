using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace POS.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();

        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
        //public DbSet<StockMovement> StockMovements => Set<StockMovement>();
        //public DbSet<Table> Tables => Set<Table>();
        //public DbSet<Order> Orders => Set<Order>();
        //public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        //public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // 👈 Required — configures Identity tables

            // Global soft delete query filter
            builder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            //builder.Entity<Order>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Category>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<ProductCategory>().HasQueryFilter(pc => !pc.Category.IsDeleted);
            builder.Entity<PurchaseOrderItem>().HasQueryFilter(poi => !poi.Product.IsDeleted);



            //ProductCategory composite key
            builder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            //AppUser
            builder.Entity<AppUser>()
                .HasOne(u => u.Branch)
                .WithMany(b => b.Users)
                .HasForeignKey(u => u.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            //Product
            builder.Entity<Product>(e =>
            {
                e.HasIndex(p => p.SKU).IsUnique();
                e.HasIndex(p => p.Barcode).IsUnique();
                e.Property(p => p.SalePrice).HasColumnType("decimal(18,2)");
                e.Property(p => p.PurchasePrice).HasColumnType("decimal(18,2)");
                e.Property(p => p.TaxRate).HasColumnType("decimal(18,2)");
            });

            //Order
            /*builder.Entity<Order>(e =>
            {
                e.HasIndex(o => o.InvoiceNumber).IsUnique();
                e.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
                e.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                e.Property(o => o.TaxAmount).HasColumnType("decimal(18,2)");
                e.Property(o => o.DiscountAmount).HasColumnType("decimal(18,2)");
                e.Property(o => o.DiscountValue).HasColumnType("decimal(18,2)");
            });*/
            // ── OrderItem decimals ────────────────────────────────────────────────
            /*builder.Entity<OrderItem>(e =>
            {
                e.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");       // ← was missing
                e.Property(i => i.DiscountAmount).HasColumnType("decimal(18,2)");  // ← was missing
                e.Property(i => i.TaxAmount).HasColumnType("decimal(18,2)");       // ← was missing
                e.Property(i => i.LineTotal).HasColumnType("decimal(18,2)");       // ← was missing
            });*/

            // ── Payment decimals ──────────────────────────────────────────────────
            /*builder.Entity<Payment>(e =>
            {
                e.Property(p => p.Amount).HasColumnType("decimal(18,2)");          // ← was missing
            });*/

            // ── PurchaseOrder decimals ────────────────────────────────────────────
            builder.Entity<PurchaseOrder>(e =>
            {
                e.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            });

            builder.Entity<PurchaseOrderItem>(e =>
            {
                e.Property(i => i.UnitCost).HasColumnType("decimal(18,2)");
            });
            // Cascade behavour
            /*builder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
