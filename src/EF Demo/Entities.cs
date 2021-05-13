using System;
using System.Collections.Generic;

namespace EF_Demo
{
    
    // abstract
    public abstract class EntityWithId
    {
        public Guid Id { get; set; }
    }
    
    public abstract class EntityWithName : EntityWithId
    {
        public string Name { get; set; }
    }

    // entities
    public class Category : EntityWithName
    {
        public ICollection<Product> Products { get; set; }
    }

    public class Product : EntityWithName
    {
        public ProductImage ProductImage { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public double Price { get; set; }
    }

    public class ProductImage : EntityWithId
    {
        public byte[] Image { get; set; }
        public Product Product { get; set; }
    }

    public class Order : EntityWithId
    {
        public int Number { get; set; }
        public DateTime Created { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem: EntityWithId
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}