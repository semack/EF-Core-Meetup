using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_Demo
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(x =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(x);
                Console.ResetColor();
            }, LogLevel.Information);
            
            optionsBuilder.UseNpgsql(
                "User ID=postgres;Server=localhost;Port=5432;Database=ef_demo;Integrated Security=true;Pooling=true;Password=Instance@1");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // defining models & relationships
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name);
                entity.Property(p => p.Name)
                    .HasMaxLength(256)
                    .IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name);
                entity.Property(p => p.Name)
                    .HasMaxLength(256)
                    .IsRequired();
                entity.Property(p => p.Price)
                    .HasPrecision(16, 2);
                entity.HasMany(x => x.Categories)
                    .WithMany(x => x.Products)
                    .UsingEntity(x => x.ToTable("ProductCategories"));
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Product)
                    .WithOne(x => x.ProductImage)
                    .HasForeignKey<ProductImage>(x => x.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Number)
                    .ValueGeneratedOnAdd();
                entity.Property(x => x.Created)
                    .HasDefaultValueSql("(now() at time zone 'utc')");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Quantity).HasDefaultValue(1);
                entity.HasOne(x => x.Product)
                    .WithMany(x => x.OrderItems)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Order)
                    .WithMany(x => x.OrderItems)
                    .HasForeignKey(x => x.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // seeding data
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());

            // inherited
            base.OnModelCreating(modelBuilder);
        }
    }
}