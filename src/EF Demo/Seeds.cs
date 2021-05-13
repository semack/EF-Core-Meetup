using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Demo
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Name = "Category 1"
                },
                new Category
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Name = "Category 2"
                },
                new Category
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    Name = "Category 3"
                }
            );
        }
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    Name = "Product 1",
                    Price = 100
                },
                new Product
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000002"),
                    Name = "Product 2",
                    Price = 1000
                },
                new Product
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000003"),
                    Name = "Product 3",
                    Price = 175
                }
            );
        }
    }

    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasData(
                new ProductImage
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001")
                },
                new ProductImage
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000002")
                },
                new ProductImage
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000003")
                }
            );
        }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasData(
                new Order
                {
                    Id = new Guid("20000000-0000-0000-0000-000000000001")
                },
                new Order
                {
                    Id = new Guid("20000000-0000-0000-0000-000000000002")
                }
            );
        }
    }

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasData(
                new OrderItem
                {
                    Id = new Guid("30000000-0000-0000-0000-000000000001"),
                    OrderId = new Guid("20000000-0000-0000-0000-000000000001"),
                    ProductId = new Guid("10000000-0000-0000-0000-000000000001"),
                    Quantity = 2
                },
                new OrderItem
                {
                    Id = new Guid("30000000-0000-0000-0000-000000000002"),
                    OrderId = new Guid("20000000-0000-0000-0000-000000000001"),
                    ProductId = new Guid("10000000-0000-0000-0000-000000000002"),
                    Quantity = 10
                },
                new OrderItem
                {
                    Id = new Guid("30000000-0000-0000-0000-000000000003"),
                    OrderId = new Guid("20000000-0000-0000-0000-000000000001"),
                    ProductId = new Guid("10000000-0000-0000-0000-000000000003"),
                    Quantity = 15
                },
                new OrderItem
                {
                    Id = new Guid("30000000-0000-0000-0000-000000000004"),
                    OrderId = new Guid("20000000-0000-0000-0000-000000000002"),
                    ProductId = new Guid("10000000-0000-0000-0000-000000000002"),
                    Quantity = 120
                },
                new OrderItem
                {
                    Id = new Guid("30000000-0000-0000-0000-000000000005"),
                    OrderId = new Guid("20000000-0000-0000-0000-000000000002"),
                    ProductId = new Guid("10000000-0000-0000-0000-000000000003"),
                    Quantity = 70
                }
            );
        }
    }
}