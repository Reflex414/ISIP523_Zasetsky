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
        static void DisplayClientRequest(TempClient client)
        {
            using (var context = Core.CreateContext())
            {
                var detailInGarage = context.DetailsGarages
                    .FirstOrDefault(dg => dg.DetailsID == client.BrokenPartID && dg.GarageID == 1);
                var detailCount = detailInGarage?.Count ?? 0;

                Console.WriteLine($"\nПриехал клиент на {client.CarModel}");
                Console.WriteLine($"Поломка: {client.BrokenPartName}");
                Console.WriteLine($"Стоимость ремонта: {client.RepairCost} руб.");
                Console.WriteLine($"На складе: {detailCount} шт.");
            }
        }

        static void AcceptOrder(TempClient client)
        {
            using (var context = Core.CreateContext())
            {
                var garage = context.Garages.First(g => g.ID == 1);
                var detailInGarage = context.DetailsGarages
                    .FirstOrDefault(dg => dg.DetailsID == client.BrokenPartID && dg.GarageID == 1);

                GameCore.CarsProcessed++;

                if (detailInGarage != null && detailInGarage.Count > 0)
                {
                    detailInGarage.Count--;

                    garage.Balance += client.RepairCost;

                    var order = new OrderHistory
                    {
                        CarModel = client.CarModel,
                        DetailID = client.BrokenPartID,
                        RepairCost = client.RepairCost,
                        Profit = client.RepairCost - context.Details.First(d => d.ID == client.BrokenPartID).Price,
                        OrderDate = DateTime.Now,
                        Status = "Completed"
                    };
                    GameCore.OrderHistory.Add(order);

                    context.SaveChanges();
                    Console.WriteLine($"Ремонт выполнен успешно! Получено: {client.RepairCost} руб.");
                }
                else
                {
                    Console.WriteLine("Нужной детали нет на складе! Производим замену случайной деталью...");

                    var randomDetail = GetRandomAvailableDetail(context);
                    if (randomDetail != null)
                    {
                        var randomDetailInGarage = context.DetailsGarages
                            .First(dg => dg.DetailsID == randomDetail.ID && dg.GarageID == 1);
                        randomDetailInGarage.Count--;

                        var penalty = 300.00m;
                        garage.Balance -= penalty;

                        var order = new OrderHistory
                        {
                            CarModel = client.CarModel,
                            DetailID = client.BrokenPartID,
                            RepairCost = 0,
                            Profit = -penalty,
                            OrderDate = DateTime.Now,
                            Status = "Failed"
                        };
                        GameCore.OrderHistory.Add(order);

                        context.SaveChanges();
                        Console.WriteLine($"Клиент недоволен! Штраф: {penalty} руб.");
                        Console.WriteLine($"Использована случайная деталь: {randomDetail.NameDetail}");
                    }
                    else
                    {
                        Console.WriteLine("На складе нет вообще никаких деталей! Штраф удвоен.");
                        garage.Balance -= 600.00m;
                        context.SaveChanges();
                    }
                }
            }
        }

        static Detail GetRandomAvailableDetail(БдДляПр7Context context)
        {
            var availableDetails = context.DetailsGarages
                .Where(dg => dg.GarageID == 1 && dg.Count > 0)
                .Select(dg => dg.DetailsID)
                .ToList();

            if (availableDetails.Any())
            {
                var random = new Random();
                var randomDetailId = availableDetails[random.Next(availableDetails.Count)];
                return context.Details.First(d => d.ID == randomDetailId);
            }
            return null;
        }
