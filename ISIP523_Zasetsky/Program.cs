using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagement
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Books,
        Food,
        Sports
    }

    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsInStock => Quantity > 0;
        public ProductCategory Category { get; set; }

        public void PrintInfo()
        {
            Console.WriteLine($"Код: {Code} | Название: {Name} | Цена: {Price:C}");
            Console.WriteLine($"Количество: {Quantity} | В наличии: {(IsInStock ? "Да" : "Нет")} | Категория: {Category}");
            Console.WriteLine(new string('-', 50));
        }
    }
    public class ProductManager
    {
        private List<Product> products = new List<Product>();
        private int lastProductId = 0;

        public void AddProduct(string name, decimal price, int quantity, ProductCategory category)
        {
            if (string.IsNullOrWhiteSpace(name) || price <= 0 || quantity < 0)
            {
                Console.WriteLine("Ошибка: Проверьте введенные данные!");
                return;
            }

            lastProductId++;
            products.Add(new Product
            {
                Code = lastProductId.ToString(),
                Name = name,
                Price = price,
                Quantity = quantity,
                Category = category
            });
            Console.WriteLine($"Товар '{name}' добавлен!");
        }

        public void RemoveProduct(string code)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                products.Remove(product);
                Console.WriteLine($"Товар с кодом {code} удален");
            }
            else
            {
                Console.WriteLine("Товар не найден");
            }
        }

    }

    