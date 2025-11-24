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

        static void DeclineOrder(TempClient client)
        {
            using (var context = Core.CreateContext())
            {
                var garage = context.Garages.First(g => g.ID == 1);
                var penalty = 100.00m;

                garage.Balance -= penalty;
                GameCore.CarsProcessed++;

                var order = new OrderHistory
                {
                    CarModel = client.CarModel,
                    DetailID = client.BrokenPartID,
                    RepairCost = 0,
                    Profit = -penalty,
                    OrderDate = DateTime.Now,
                    Status = "Declined"
                };
                GameCore.OrderHistory.Add(order);
                context.SaveChanges();

                Console.WriteLine($"Заказ отклонен. Штраф: {penalty} руб.");
            }
        }

        static void ShowPurchaseMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ПОКУПКА ЗАПЧАСТЕЙ ===");

            using (var context = Core.CreateContext())
            {
                var details = context.Details.ToList();
                var garage = context.Garages.First(g => g.ID == 1);

                for (int i = 0; i < details.Count; i++)
                {
                    var detail = details[i];
                    var detailInGarage = context.DetailsGarages
                        .FirstOrDefault(dg => dg.DetailsID == detail.ID && dg.GarageID == 1);
                    var detailCount = detailInGarage?.Count ?? 0;

                    Console.WriteLine($"{i + 1}. {detail.NameDetail} - {detail.Price} руб. (на складе: {detailCount})");
                }

                Console.Write("\nВыберите номер запчасти: ");
                if (int.TryParse(Console.ReadLine(), out int detailIndex) && detailIndex >= 1 && detailIndex <= details.Count)
                {
                    var selectedDetail = details[detailIndex - 1];

                    Console.Write("Введите количество: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        var totalCost = selectedDetail.Price * quantity;

                        if (garage.Balance >= totalCost)
                        {
                            garage.Balance -= totalCost;

                            var delivery = new PendingDelivery
                            {
                                DetailID = selectedDetail.ID,
                                DetailName = selectedDetail.NameDetail,
                                Quantity = quantity,
                                TotalCost = totalCost,
                                OrderPlacedAtCar = GameCore.CarsProcessed
                            };

                            GameCore.PendingDeliveries.Add(delivery);
                            context.SaveChanges();

                            Console.WriteLine($"Заказ оформлен! Поставка через 2 машины. Списано: {totalCost} руб.");
                        }
                        else
                        {
                            Console.WriteLine("Недостаточно средств!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверное количество!");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор!");
                }
            }
        }

        static void ShowWarehouseStatus()
        {
            Console.Clear();
            Console.WriteLine("=== СКЛАД ГАРАЖА ===");

            using (var context = Core.CreateContext())
            {
                var detailsInGarage = context.DetailsGarages
                    .Where(dg => dg.GarageID == 1)
                    .Join(context.Details,
                          dg => dg.DetailsID,
                          d => d.ID,
                          (dg, d) => new { Detail = d, Count = dg.Count })
                    .ToList();

                foreach (var item in detailsInGarage)
                {
                    Console.WriteLine($"{item.Detail.NameDetail}: {item.Count} шт. (цена: {item.Detail.Price} руб.)");
                }

                if (GameCore.PendingDeliveries.Any())
                {
                    Console.WriteLine("\n=== ОЖИДАЮЩИЕ ПОСТАВКИ ===");
                    foreach (var delivery in GameCore.PendingDeliveries)
                    {
                        Console.WriteLine($"{delivery.DetailName}: {delivery.Quantity} шт. (поставка через {delivery.OrderPlacedAtCar + 2 - GameCore.CarsProcessed} машин)");
                    }
                }

                var lowStock = detailsInGarage.Where(d => d.Count < 3).ToList();
                if (lowStock.Any())
                {
                    Console.WriteLine("\n!!! НИЗКИЙ ЗАПАС !!!");
                    foreach (var item in lowStock)
                    {
                        Console.WriteLine($"{item.Detail.NameDetail}: осталось {item.Count} шт.");
                    }
                }
            }
        }
        static void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("=== СТАТИСТИКА ===");

            using (var context = Core.CreateContext())
            {
                var garage = context.Garages.First(g => g.ID == 1);
                var totalOrders = GameCore.OrderHistory.Count;
                var completedOrders = GameCore.OrderHistory.Count(o => o.Status == "Completed");
                var failedOrders = GameCore.OrderHistory.Count(o => o.Status == "Failed");
                var declinedOrders = GameCore.OrderHistory.Count(o => o.Status == "Declined");

                var totalProfit = GameCore.OrderHistory.Sum(o => o.Profit);
                var totalRevenue = GameCore.OrderHistory.Sum(o => o.RepairCost);

                Console.WriteLine($"Всего заказов: {totalOrders}");
                Console.WriteLine($"Успешных ремонтов: {completedOrders}");
                Console.WriteLine($"Неудачных ремонтов: {failedOrders}");
                Console.WriteLine($"Отклоненных заказов: {declinedOrders}");
                Console.WriteLine($"Общий доход: {totalRevenue} руб.");
                Console.WriteLine($"Общая прибыль: {totalProfit} руб.");
                Console.WriteLine($"Текущий баланс: {garage.Balance} руб.");

                if (GameCore.OrderHistory.Any())
                {
                    var popularDetails = GameCore.OrderHistory
                        .Where(o => o.Status == "Completed")
                        .GroupBy(o => o.DetailID)
                        .Select(g => new { DetailID = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .Take(3)
                        .ToList();

                    Console.WriteLine("\nСамые частые поломки:");
                    foreach (var item in popularDetails)
                    {
                        var detail = context.Details.First(d => d.ID == item.DetailID);
                        Console.WriteLine($"  {detail.NameDetail}: {item.Count} раз");
                    }
                }
            }
        }

        static void ProcessDeliveries()
        {
            var deliveriesToProcess = GameCore.PendingDeliveries
                .Where(d => GameCore.CarsProcessed >= d.OrderPlacedAtCar + 2)
                .ToList();

            if (deliveriesToProcess.Any())
            {
                using (var context = Core.CreateContext())
                {
                    var garage = context.Garages.First(g => g.ID == 1);

                    foreach (var delivery in deliveriesToProcess)
                    {
                        var detailInGarage = context.DetailsGarages
                            .FirstOrDefault(dg => dg.DetailsID == delivery.DetailID && dg.GarageID == 1);

                        if (detailInGarage != null)
                        {
                            detailInGarage.Count += delivery.Quantity;
                        }
                        else
                        {
                            detailInGarage = new DetailsGarage
                            {
                                GarageID = garage.ID,
                                DetailsID = delivery.DetailID,
                                Count = delivery.Quantity
                            };
                            context.DetailsGarages.Add(detailInGarage);
                        }

                        Console.WriteLine($"Поставка получена: {delivery.DetailName} - {delivery.Quantity} шт.");
                        GameCore.PendingDeliveries.Remove(delivery);
                    }

                    context.SaveChanges();
                }
            }
        }

        static void CheckGameOver()
        {
            using (var context = Core.CreateContext())
            {
                var garage = context.Garages.First(g => g.ID == 1);

                if (garage.Balance <= 0)
                {
                    Console.WriteLine("\nИГРА ОКОНЧЕНА! Вы банкрот!!!");
                    Console.WriteLine($"Итоговый счет: Успешных ремонтов - {GameCore.OrderHistory.Count(o => o.Status == "Completed")}");
                    Console.WriteLine($"Всего обработано машин: {GameCore.CarsProcessed}");
                    Environment.Exit(0);
                }

                if (garage.Balance >= 1000000.00m)
                {
                    Console.WriteLine("\nПОБЕДА! Вы заработали 1000000 рублей!");
                    Console.WriteLine($"Итоговый счет: Успешных ремонтов - {GameCore.OrderHistory.Count(o => o.Status == "Completed")}");
                    Console.WriteLine($"Всего обработано машин: {GameCore.CarsProcessed}");
                    Environment.Exit(0);
                }
            }
        }
    }
    public class TempClient
    {
        public string CarModel { get; set; }
        public int BrokenPartID { get; set; }
        public string BrokenPartName { get; set; }
        public decimal RepairCost { get; set; }
    }

    public class PendingDelivery
    {
        public int DetailID { get; set; }
        public string DetailName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public int OrderPlacedAtCar { get; set; }
    }

    public class OrderHistory
    {
        public string CarModel { get; set; }
        public int DetailID { get; set; }
        public decimal RepairCost { get; set; }
        public decimal Profit { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } //Completed, Failed, Declined
    }

    public static class GameCore
    {
        public static int CarsProcessed { get; set; } = 0;
        public static List<PendingDelivery> PendingDeliveries { get; set; } = new List<PendingDelivery>();
        public static List<OrderHistory> OrderHistory { get; set; } = new List<OrderHistory>();
    }
}