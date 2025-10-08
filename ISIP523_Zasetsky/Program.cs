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
        public void OrderSupply(string code, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null && quantity > 0)
            {
                product.Quantity += quantity;
                Console.WriteLine($"Поставка: +{quantity}. Теперь: {product.Quantity}");
            }
            else
            {
                Console.WriteLine("Ошибка поставки");
            }
        }

        public void SellProduct(string code, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар не найден");
                return;
            }

            if (product.Quantity >= quantity && quantity > 0)
            {
                product.Quantity -= quantity;
                Console.WriteLine($"Продажа: -{quantity}. Остаток: {product.Quantity}");
                Console.WriteLine($"Сумма: {product.Price * quantity:C}");
            }
            else
            {
                Console.WriteLine($"Недостаточно товара! В наличии: {product.Quantity}");
            }
        }

        public void SearchByCode(string code)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null) product.PrintInfo();
            else Console.WriteLine("Товар не найден");
        }

        public void SearchByName(string name)
        {
            var found = products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            DisplaySearchResults(found, $"по названию '{name}'");
        }

        public void SearchByCategory(ProductCategory category)
        {
            var found = products.Where(p => p.Category == category).ToList();
            DisplaySearchResults(found, $"в категории '{category}'");
        }

        public void DisplayAllProducts()
        {
            DisplaySearchResults(products, "всего");
        }

        private void DisplaySearchResults(List<Product> foundProducts, string searchType)
        {
            if (foundProducts.Any())
            {
                Console.WriteLine($"\nНайдено товаров {searchType}: {foundProducts.Count}");
                foundProducts.ForEach(p => p.PrintInfo());
            }
            else
            {
                Console.WriteLine("Товары не найдены");
            }
        }
    }

    