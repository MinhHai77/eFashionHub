using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopHai.Models;

public partial class ShopClothesContext : DbContext
{
    public ShopClothesContext()
    {
    }

    public ShopClothesContext(DbContextOptions<ShopClothesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-APBIJBB\\SQLEXPRESS;uid=sa;password=123;database=ShopClothes;Encrypt=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B7286302EC");

            entity.ToTable("Cart");

            entity.Property(e => e.IsPaid).HasColumnName("isPaid");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Cart_Product");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId);

            entity.Property(e => e.CateId).HasColumnName("cateId");
            entity.Property(e => e.CateName)
                .HasMaxLength(50)
                .HasColumnName("cateName");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("invoiceId");
            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.InvoiceDate)
                .HasColumnType("datetime")
                .HasColumnName("invoiceDate");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalAmount");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Cart).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_Invoice_Cart");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Invoice_User");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .HasColumnName("brand");
            entity.Property(e => e.CateId).HasColumnName("cateId");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductImage).HasColumnName("productImage");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .HasColumnName("productName");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .HasColumnName("size");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK_Product_Categories");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
