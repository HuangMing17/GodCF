using GodCF.Models;

namespace GodCF.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Create the database if it doesn't exist
            context.Database.EnsureCreated();

            // Check if we already have data
            if (context.Products.Any() || context.Categories.Any())
            {
                return; // Database has been seeded
            }

            // Add Categories
            var categories = new Category[]
            {
                new Category { Name = "Cà phê", Code = "coffee", Description = "Các loại cà phê" },
                new Category { Name = "Trà", Code = "tea", Description = "Các loại trà" },
                new Category { Name = "Bánh ngọt", Code = "pastries", Description = "Bánh ngọt các loại" },
                new Category { Name = "Sinh tố", Code = "smoothies", Description = "Các loại sinh tố" }
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            // Add Products
            var products = new Product[]
            {
                new Product {
                    Name = "Cà phê sữa",
                    Description = "Cà phê đậm đà với sữa đặc thơm béo",
                    Price = 35000,
                    ImageUrl = "/images/menu/cafe-sua.jpg",
                    CategoryId = categories[0].Id
                },
                new Product {
                    Name = "Cà phê đen",
                    Description = "Cà phê nguyên chất đậm đà",
                    Price = 29000,
                    ImageUrl = "/images/menu/cafe-den.jpg",
                    CategoryId = categories[0].Id
                },
                new Product {
                    Name = "Trà đào",
                    Description = "Trà đào thơm mát với đào tươi",
                    Price = 45000,
                    ImageUrl = "/images/menu/tra-dao.jpg",
                    CategoryId = categories[1].Id
                },
                new Product {
                    Name = "Tiramisu",
                    Description = "Bánh tiramisu thơm ngon",
                    Price = 55000,
                    ImageUrl = "/images/menu/tiramisu.jpg",
                    CategoryId = categories[2].Id
                },
                new Product {
                    Name = "Sinh tố dâu",
                    Description = "Sinh tố dâu tươi ngon",
                    Price = 49000,
                    ImageUrl = "/images/menu/sinh-to-dau.jpg",
                    CategoryId = categories[3].Id
                }
            };

            foreach (var p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();

            // Add Tables
            var tables = new Table[]
            {
                new Table { TableNumber = 1, Capacity = 2, Location = "Tầng 1" },
                new Table { TableNumber = 2, Capacity = 2, Location = "Tầng 1" },
                new Table { TableNumber = 3, Capacity = 4, Location = "Tầng 1" },
                new Table { TableNumber = 4, Capacity = 4, Location = "Tầng 1" },
                new Table { TableNumber = 5, Capacity = 6, Location = "Tầng 1" },
                new Table { TableNumber = 6, Capacity = 6, Location = "Tầng 1" },
                new Table { TableNumber = 7, Capacity = 4, Location = "Tầng 2" },
                new Table { TableNumber = 8, Capacity = 4, Location = "Tầng 2" },
                new Table { TableNumber = 9, Capacity = 8, Location = "Tầng 2" },
                new Table { TableNumber = 10, Capacity = 8, Location = "Tầng 2" }
            };

            foreach (var t in tables)
            {
                context.Tables.Add(t);
            }
            context.SaveChanges();
        }
    }
}