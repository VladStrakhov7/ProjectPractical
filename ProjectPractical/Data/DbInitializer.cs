using Microsoft.AspNetCore.Identity;
using ProjectPractical.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPractical.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1. Убедимся, что база создана
            context.Database.EnsureCreated();

            // 2. Создаем роли, если их нет
            string[] roles = { "Admin", "User", "Manager" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 3. Создаем администратора
            var adminEmail = "admin@example.com";
            var adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    await userManager.AddToRoleAsync(admin, "Manager");
                }
            }

            // 4. Добавляем категории
            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "Электроника" },
                    new Category { Name = "Одежда" },
                    new Category { Name = "Книги" },
                    new Category { Name = "Спорт" }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // 5. Добавляем продукты
            if (!context.Products.Any())
            {
                var electronics = context.Categories.First(c => c.Name == "Электроника");
                var books = context.Categories.First(c => c.Name == "Книги");

                var products = new[]
                {
                    new Product { Name = "Ноутбук", Description = "Игровой ноутбук", Price = 74999.99m, CategoryId = electronics.Id },
                    new Product { Name = "Смартфон", Description = "Флагманский смартфон", Price = 89999.50m, CategoryId = electronics.Id },
                    new Product { Name = "Наушники", Description = "Беспроводные наушники", Price = 7999.00m, CategoryId = electronics.Id },
                    new Product { Name = "Книга по программированию", Description = "C# для начинающих", Price = 2499.00m, CategoryId = books.Id },
                    new Product { Name = "Футболка", Description = "Хлопковая футболка", Price = 1299.00m, CategoryId = 2 } // Одежда
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("✅ База данных успешно заполнена!");
        }
    }
}