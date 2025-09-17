//Создать консольное приложение для подсчета потраченных за день средств. 
//Пользователь вводит количество операций, которые будут записаны.
//Можно внести от 2 до 40 операций.
//Дальше, пользователь по шаблону (Название услуги или товара; Количество денег); вводит траты. Валюта - рубли.
//Пример: (Влажные салфетки "Лента"; 235);
//После заполнения всех трат, пользователь должен увидеть следующее меню:
//1.Вывод данных
//2.Статистика(среднее, максимальное, минимальное, сумма);
//3.Сортировка по цене(пузырьковая сортировка);
//4.Конвертация валюты(пользователь вводит курс или выбирает из списка);
//5.Поиск по названию 0. Выход
//Выбор пунктов меню осуществляется по соответствующей цифре.
using System;

class Program
{
    static void Main()
    {
        int n = 0;
        while (n < 2 || n > 40)
        {
            Console.Write("Введите количество операций (2-40): ");
            n = Convert.ToInt32(Console.ReadLine());
            if (n < 2 || n > 40)
                Console.WriteLine("От 2 до 40!");
        }
        string[] names = new string[n];
        decimal[] costs = new decimal[n];

        for (int i = 0; i < n; i++)
        {
            Console.Write("Введите операцию (Название; Сумма): ");
            string[] input = Console.ReadLine().Split(';');
            names[i] = input[0].Trim();
            costs[i] = Convert.ToDecimal(input[1].Trim());
        }

        Console.WriteLine("Данные сохранены!\n");


            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика");
                Console.WriteLine("3. Сортировка по цене");
                Console.WriteLine("4. Конвертация валюты");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowData();
                        break;
                    case "2":
                        ShowStats();
                        break;
                    case "3":
                        SortCosts(); 
                        ShowData();
                        break;
                    case "4":
                        ConvertCurrency();
                        break;
                    case "5":
                        SearchName();
                        break;
                    case "0":
                        Console.WriteLine("Выход...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!\n");
                        break;
                }
            }
        }
    }
