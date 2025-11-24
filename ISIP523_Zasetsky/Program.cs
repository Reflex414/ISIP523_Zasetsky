using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace ISIP523_Zasetsky
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeGame();

            while (true)
            {
                Console.Clear();
                DisplayGameStatus();

                var clientInfo = GenerateRandomClient();
                DisplayClientRequest(clientInfo);

                DisplayActionMenu();
                var choice = GetUserChoice();

                switch (choice)
                {
                    case 1: AcceptOrder(clientInfo); break;
                    case 2: DeclineOrder(clientInfo); break;
                    case 3: ShowPurchaseMenu(); break;
                    case 4: ShowWarehouseStatus(); break;
                    case 5: ShowStatistics(); break;
                    case 6: return;
                    default: Console.WriteLine("Неверный выбор!"); break;
                }

                ProcessDeliveries();
                CheckGameOver();

                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        static void InitializeGame()
        {
            using (var context = Core.CreateContext())
            {
                var garage = context.Garages.FirstOrDefault(g => g.ID == 1);
                if (garage == null)
                {
                    garage = new Garage
                    {
                        ID = 1,
                        NameGarage = "Главный гараж",
                        Balance = 10000.00m  // ← ВОТ ЗДЕСЬ установлен начальный баланс 10000
                    };
                    context.Garages.Add(garage);
                    context.SaveChanges();
                }

                Console.WriteLine($"Игра 'Автосервис' запущена! Гараж: {garage.NameGarage}, Баланс: {garage.Balance} руб.");
            }
        }

        static void DisplayGameStatus()
        {
            using (var context = Core.CreateContext())
            {
                var garage = context.Garages.First(g => g.ID == 1);
                var totalDetails = context.DetailsGarages.Sum(dg => dg.Count);

                Console.WriteLine("=== АВТОСЕРВИС ===");
                Console.WriteLine($"Гараж: {garage.NameGarage}");
                Console.WriteLine($"Баланс: {garage.Balance} руб.");
                Console.WriteLine($"Всего деталей на складе: {totalDetails}");
                Console.WriteLine($"Ожидающих поставок: {GameCore.PendingDeliveries.Count}");
                Console.WriteLine($"Обработано машин: {GameCore.CarsProcessed}");
                Console.WriteLine("===================");
            }
        }

        static void DisplayActionMenu()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Принять заказ");
            Console.WriteLine("2 - Отказаться от заказа");
            Console.WriteLine("3 - Купить запчасти");
            Console.WriteLine("4 - Показать склад");
            Console.WriteLine("5 - Статистика");
            Console.WriteLine("6 - Выйти из игры");
        }

        static int GetUserChoice()
        {
            Console.Write("Ваш выбор: ");
            return int.TryParse(Console.ReadLine(), out int choice) ? choice : 0;
        }

        static TempClient GenerateRandomClient()
        {
            var random = new Random();

            using (var context = Core.CreateContext())
            {
                var details = context.Details.ToList();
                var carModels = new List<string>
                {
                    "Toyota Camry 2020",
                    "Honda Civic 2019",
                    "BMW X5 2021",
                    "Mercedes C-class 2022",
                    "Audi A4 2021",
                    "Ford Focus 2018",
                    "Volkswagen Golf 2019",
                    "Hyundai Solaris 2020"
                };

                var randomDetail = details[random.Next(details.Count)];
                var randomCar = carModels[random.Next(carModels.Count)];

                var repairCost = randomDetail.Price * 1.8m;

                return new TempClient
                {
                    CarModel = randomCar,
                    BrokenPartID = randomDetail.ID,
                    BrokenPartName = randomDetail.NameDetail,
                    RepairCost = repairCost
                };
            }
        }
