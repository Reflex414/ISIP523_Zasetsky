using System;
using System.Collections.Generic;
using System.Linq;

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
