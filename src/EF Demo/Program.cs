using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EF_Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello Entity Framework Core!");

            
            RecreateAndSeed();

            //LinqSyntax();

            ExpressionIssue();
            // var data = GetContextAlreadyDisposed();
            // Console.WriteLine(data.First().ToJson());
            // CartesianExplosion();
            // LazyLoadingIssue();
            // ToListFilteringIssue();
            //QueryCachingIssue();

            var id = AddProduct("Test Product", 123.12);
            
            UpdateProductFromDb(id, "New Product Name 1", 521.12);
            UpdateProductByUpdateMethod(id, "New Product Name 2", 321.00);
            UpdateProductByAttachMethod(id, "New Product Name 3");
            
            //CreateOrderDemo(id, 123);
            RemoveProduct(id);

            Console.WriteLine("Done.");
        }

        private static void LinqSyntax()
        {
            using (var context = new DataContext())
            {
                var querySyntaxResult = from p in context.Products
                    where p.Name.Contains("Product 1")
                    select p;

                var methodSyntaxResult = context.Products
                    .Where(x => x.Name.Contains("Product 1"));
            }
        }

        #region Issues

        private static void CartesianExplosion()
        {
            using (var context = new DataContext())
            {
                var products = context.Products
                    .Include(x => x.OrderItems)
                    .AsSplitQuery();
                foreach (var product in products)
                {
                    foreach (var item in product.OrderItems)
                    {
                        Console.WriteLine(item.Product.Name.ToJson());
                    }
                }
            }
        }

        private static void ToListFilteringIssue()
        {
            using (var context = new DataContext())
            {
                // incorrect
                var products = context.Products
                    .ToList()
                    .Where(x => x.Name.Equals("Product 1"));

                //correct
                products = context.Products
                    .Where(x => x.Name.Equals("Product 1"))
                    .ToList();
            }
        }

        private static void QueryCachingIssue()
        {
            using (var context = new DataContext())
            {
                // incorrect
                var products = context.Products
                    .Where(x => x.Name.Equals("Product 1"))
                    .ToList();

                products = context.Products
                    .Where(x => x.Name.Equals("Product 2"))
                    .ToList();

                //correct
                var condition = "Product 1";
                products = context.Products
                    .Where(x => x.Name.Equals(condition))
                    .ToList();

                condition = "Product 2";
                products = context.Products
                    .Where(x => x.Name.Equals(condition))
                    .ToList();
            }
        }

        private static void LazyLoadingIssue()
        {
            using (var context = new DataContext())
            {
                var products = context.Products
                    .Include(x => x.OrderItems);

                // each DB call on product
                foreach (var product in products)
                {
                    foreach (var item in product.OrderItems)
                    {
                        // each DB call on OrderItems
                        Console.WriteLine(item.Product.Name.ToJson());
                    }
                }
            }
        }

        private static IEnumerable<Product> GetContextAlreadyDisposed()
        {
            using (var context = new DataContext())
            {
                return context.Products;
            }
        }

        private static void ExpressionIssue()
        {
            using (var context = new DataContext())
            {
                // incorrect
                var data1 = context.Products
                    .Where(x => x.Name.Compare("Product 1"))
                    .ToList();

                //correct 
                var data2 = context.Products
                    .AsEnumerable()
                    .Where(x => x.Name.Compare("Product 1"))
                    .ToList();
            }
        }

        #endregion

        #region Demo

        private static void RecreateAndSeed()
        {
            using (var context = new DataContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }
        }

        private static Guid AddProduct(string name, double price)
        {
            var id = Guid.NewGuid();
            using (var context = new DataContext())
            {
                //adding
                var product = new Product
                {
                    Id = id,
                    Name = name,
                    Price = price,
                    ProductImage = new ProductImage()
                };
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.Products.Add(product);
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.SaveChanges();
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
            }

            ShowResults();
            return id;
        }

        private static void UpdateProductFromDb(Guid id, string name, double price)
        {
            using (var context = new DataContext())
            {
                var product = context.Products.Single(x => x.Id == id);
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                product.Name = name;
                product.Price = price;
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.SaveChanges();
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
            }

            ShowResults();
        }

        private static void UpdateProductByUpdateMethod(Guid id, string name, double price)
        {
            using (var context = new DataContext())
            {
                var product = new Product
                {
                    Id = id,
                    Name = name,
                    Price = price
                };
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.Update(product);
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.SaveChanges();
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
            }

            ShowResults();
        }

        private static void UpdateProductByAttachMethod(Guid id, string name)
        {
            using (var context = new DataContext())
            {
                var product = new Product
                {
                    Id = id,
                };
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.Products.Attach(product);
                product.Name = name;
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.SaveChanges();
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
            }

            ShowResults();
        }

        private static void RemoveProduct(Guid id)
        {
            using (var context = new DataContext())
            {
                // removing
                var product = context.Products.Find(id);
                context.Products.Remove(product);
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
                context.SaveChanges();
                Console.WriteLine($"EntityState: {context.Entry(product).State}");
            }

            ShowResults();
        }

        private static void ShowResults()
        {
            using (var context = new DataContext())
            {
                var items = context.Products
                    .AsNoTracking()
                    .Include(s => s.ProductImage)
                    .Select(x => new {x.Name, x.Price, x.ProductImage.Id})
                    .ToList();
                
                items.ForEach(x => Console.WriteLine(x.ToJson()));
            }
        }

        private static void CreateOrderDemo(Guid productId, int quantity)
        {
            using (var context = new DataContext())
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                };
                // context.Orders.Add(order);
                var item = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Order = order,
                    Quantity = quantity
                };
                context.OrderItems.Add(item);
                context.SaveChanges();

                var results = context.Orders
                    .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                    .ToList();
            }
        }

        #endregion
    }
}