using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;


namespace CafePOS.Models.Database;

public partial class CafeDbContext : DbContext
{
    public CafeDbContext()
    {
    }

    public CafeDbContext(DbContextOptions<CafeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CafeOrder> CafeOrders { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemPrice> ItemPrices { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<TimeOfDay> TimeOfDays { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CafeOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("CafeOrder");

            entity.HasIndex(e => e.PaymentTypeId, "PaymentTypeID");

            entity.HasIndex(e => e.ServerId, "ServerID");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.AmountDue).HasPrecision(6, 2);
            entity.Property(e => e.OrderDate).HasMaxLength(6);
            entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
            entity.Property(e => e.ServerId).HasColumnName("ServerID");
            entity.Property(e => e.SubTotal).HasPrecision(6, 2);
            entity.Property(e => e.Tax).HasPrecision(6, 2);
            entity.Property(e => e.Tip).HasPrecision(6, 2);

            entity.HasOne(d => d.PaymentType).WithMany(p => p.CafeOrders)
                .HasForeignKey(d => d.PaymentTypeId)
                .HasConstraintName("CafeOrder_ibfk_2");

            entity.HasOne(d => d.Server).WithMany(p => p.CafeOrders)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("CafeOrder_ibfk_1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("Item");

            entity.HasIndex(e => e.CategoryId, "CategoryID");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ItemDescription)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ItemName)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Item_ibfk_1");
        });

        modelBuilder.Entity<ItemPrice>(entity =>
        {
            entity.HasKey(e => e.ItemPriceId).HasName("PRIMARY");

            entity.ToTable("ItemPrice");

            entity.HasIndex(e => e.ItemId, "ItemID");

            entity.HasIndex(e => e.TimeOfDayId, "TimeOfDayID");

            entity.Property(e => e.ItemPriceId).HasColumnName("ItemPriceID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Price).HasPrecision(5, 2);
            entity.Property(e => e.TimeOfDayId).HasColumnName("TimeOfDayID");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemPrices)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ItemPrice_ibfk_1");

            entity.HasOne(d => d.TimeOfDay).WithMany(p => p.ItemPrices)
                .HasForeignKey(d => d.TimeOfDayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ItemPrice_ibfk_2");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PRIMARY");

            entity.ToTable("OrderItem");

            entity.HasIndex(e => e.ItemPriceId, "ItemPriceID");

            entity.HasIndex(e => e.OrderId, "OrderID");

            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.ExtendedPrice).HasPrecision(6, 2);
            entity.Property(e => e.ItemPriceId).HasColumnName("ItemPriceID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.ItemPrice).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ItemPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderItem_ibfk_2");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderItem_ibfk_1");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.PaymentTypeId).HasName("PRIMARY");

            entity.ToTable("PaymentType");

            entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
            entity.Property(e => e.PaymentTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.ServerId).HasName("PRIMARY");

            entity.ToTable("Server");

            entity.Property(e => e.ServerId).HasColumnName("ServerID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(25)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<TimeOfDay>(entity =>
        {
            entity.HasKey(e => e.TimeOfDayId).HasName("PRIMARY");

            entity.ToTable("TimeOfDay");

            entity.Property(e => e.TimeOfDayId).HasColumnName("TimeOfDayID");
            entity.Property(e => e.TimeOfDayName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
