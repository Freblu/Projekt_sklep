using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Infrastructure.Database;

internal sealed class PartsComDbContext(DbContextOptions<PartsComDbContext> options) : DbContext(options), IUnitOfWork
{
    // Existing
    internal DbSet<User> Users { get; set; }
    internal DbSet<ProductCategory> ProductCategories { get; set; }
    internal DbSet<ProductSubCategory> ProductSubCategories { get; set; }

    // Products
    internal DbSet<Product> Products { get; set; }
    internal DbSet<ProductImage> ProductImages { get; set; }
    internal DbSet<ProductTag> ProductTags { get; set; }
    internal DbSet<ProductProductTag> ProductProductTags { get; set; }
    internal DbSet<ProductReview> ProductReviews { get; set; }

    // Cart & Orders
    internal DbSet<Cart> Carts { get; set; }
    internal DbSet<CartItem> CartItems { get; set; }
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<OrderItem> OrderItems { get; set; }

    // User Related
    internal DbSet<Address> Addresses { get; set; }
    internal DbSet<PaymentCard> PaymentCards { get; set; }
    internal DbSet<BrowsingHistory> BrowsingHistories { get; set; }

    // Roles & Permissions
    internal DbSet<Role> Roles { get; set; }
    internal DbSet<Permission> Permissions { get; set; }
    internal DbSet<UserRole> UserRoles { get; set; }
    internal DbSet<RolePermission> RolePermissions { get; set; }

    // Content
    internal DbSet<NewsPost> NewsPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureProductCategory(modelBuilder);
        ConfigureProduct(modelBuilder);
        ConfigureProductProductTag(modelBuilder);
        ConfigureProductReview(modelBuilder);
        ConfigureCart(modelBuilder);
        ConfigureOrder(modelBuilder);
        ConfigureAddress(modelBuilder);
        ConfigureUserRole(modelBuilder);
        ConfigureRolePermission(modelBuilder);
        ConfigureBrowsingHistory(modelBuilder);
    }

    private static void ConfigureProductCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>()
            .HasMany(c => c.SubCategories)
            .WithOne(p => p.ProductCategory)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductCategory)
            .WithMany()
            .HasForeignKey(p => p.ProductCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductSubCategory)
            .WithMany()
            .HasForeignKey(p => p.ProductSubCategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureProductProductTag(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductProductTag>()
            .HasKey(pt => new { pt.ProductId, pt.ProductTagId });

        modelBuilder.Entity<ProductProductTag>()
            .HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(pt => pt.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductProductTag>()
            .HasOne(pt => pt.ProductTag)
            .WithMany(t => t.ProductProductTags)
            .HasForeignKey(pt => pt.ProductTagId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureProductReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductReview>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureCart(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Cart>()
            .HasMany(c => c.Items)
            .WithOne(i => i.Cart)
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShippingAddress)
            .WithMany(a => a.ShippingOrders)
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.BillingAddress)
            .WithMany(a => a.BillingOrders)
            .HasForeignKey(o => o.BillingAddressId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureAddress(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PaymentCard>()
            .HasOne(pc => pc.User)
            .WithMany(u => u.PaymentCards)
            .HasForeignKey(pc => pc.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureUserRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureRolePermission(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureBrowsingHistory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrowsingHistory>()
            .HasOne(bh => bh.User)
            .WithMany(u => u.BrowsingHistories)
            .HasForeignKey(bh => bh.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<BrowsingHistory>()
            .HasOne(bh => bh.Product)
            .WithMany(p => p.BrowsingHistories)
            .HasForeignKey(bh => bh.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
