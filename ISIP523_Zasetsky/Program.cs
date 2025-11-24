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

        