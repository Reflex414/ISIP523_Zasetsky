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
    class Program
    {
        static ProductManager manager = new ProductManager();

        static void Main(string[] args)
        {
            InitializeTestData();

            while (true)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddProductMenu(); break;
                    case "2": RemoveProductMenu(); break;
                    case "3": OrderSupplyMenu(); break;
                    case "4": SellProductMenu(); break;
                    case "5": SearchMenu(); break;
                    case "6": manager.DisplayAllProducts(); break;
                    case "7": return;
                    default: Console.WriteLine("Неверный выбор!"); break;
                }

                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void InitializeTestData()
        {
            manager.AddProduct("Смартфон Samsung", 25000, 10, ProductCategory.Electronics);
            manager.AddProduct("Футболка хлопковая", 1500, 25, ProductCategory.Clothing);
            manager.AddProduct("Война и мир", 800, 15, ProductCategory.Books);
            manager.AddProduct("Шоколад Alpen Gold", 120, 50, ProductCategory.Food);
            manager.AddProduct("Футбольный мяч", 3000, 8, ProductCategory.Sports);
            Console.WriteLine("Тестовые данные загружены!\n");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=== СИСТЕМА УЧЁТА ТОВАРОВ ===");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товаров");
            Console.WriteLine("6. Показать все товары");
            Console.WriteLine("7. Выход");
            Console.Write("Выберите действие: ");
        }

        static void AddProductMenu()
        {
            Console.Write("Название: ");
            string name = Console.ReadLine();

            Console.Write("Цена: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            { Console.WriteLine("Ошибка цены!"); return; }

            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            { Console.WriteLine("Ошибка количества!"); return; }

            Console.WriteLine("Категории: 1-Электроника, 2-Одежда, 3-Книги, 4-Еда, 5-Спорт");
            if (!int.TryParse(Console.ReadLine(), out int cat) || cat < 1 || cat > 5)
            { Console.WriteLine("Ошибка категории!"); return; }

            manager.AddProduct(name, price, quantity, (ProductCategory)(cat - 1));
        }

        static void RemoveProductMenu()
        {
            Console.Write("Код товара: ");
            manager.RemoveProduct(Console.ReadLine());
        }

        static void OrderSupplyMenu()
        {
            Console.Write("Код товара: ");
            string code = Console.ReadLine();
            Console.Write("Количество: ");
            if (int.TryParse(Console.ReadLine(), out int quantity))
                manager.OrderSupply(code, quantity);
            else
                Console.WriteLine("Ошибка количества!");
        }

        static void SellProductMenu()
        {
            Console.Write("Код товара: ");
            string code = Console.ReadLine();
            Console.Write("Количество: ");
            if (int.TryParse(Console.ReadLine(), out int quantity))
                manager.SellProduct(code, quantity);
            else
                Console.WriteLine("Ошибка количества!");
        }

        static void SearchMenu()
        {
            Console.WriteLine("Поиск по: 1-Коду, 2-Названию, 3-Категории");
            var type = Console.ReadLine();

            switch (type)
            {
                case "1":
                    Console.Write("Код: ");
                    manager.SearchByCode(Console.ReadLine());
                    break;
                case "2":
                    Console.Write("Название: ");
                    manager.SearchByName(Console.ReadLine());
                    break;
                case "3":
                    Console.WriteLine("Категории: 1-Электроника, 2-Одежда, 3-Книги, 4-Еда, 5-Спорт");
                    if (int.TryParse(Console.ReadLine(), out int cat) && cat >= 1 && cat <= 5)
                        manager.SearchByCategory((ProductCategory)(cat - 1));
                    else
                        Console.WriteLine("Ошибка категории!");
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }
}

