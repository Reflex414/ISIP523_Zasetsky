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
    static void ShowData(string[] names, decimal[] costs, int n)
    {
        Console.WriteLine("\nВаши операции:");
        for (int i = 0; i < n; i++)
            Console.WriteLine($"{names[i]} - {costs[i]} руб.");
        Console.WriteLine();
    }

    static void ShowStats(decimal[] costs, int n)
    {
        decimal sum = 0, max = 0, min = decimal.MaxValue;
        foreach (decimal cost in costs)
        {
            sum += cost;
            if (cost > max)
                max = cost;
            if (cost < min)
                min = cost;
        }
        Console.WriteLine($"\nСумма: {sum} руб.");
        Console.WriteLine($"Среднее: {sum / n} руб.");
        Console.WriteLine($"Макс: {max} руб.");
        Console.WriteLine($"Мин: {min} руб.\n");
    }
    static void SortCosts(string[] names, decimal[] costs, int n)
    {
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (costs[j] > costs[j + 1])
                {
                    (costs[j], costs[j + 1]) = (costs[j + 1], costs[j]);
                    (names[j], names[j + 1]) = (names[j + 1], names[j]);
                }
            }
        }
        Console.WriteLine("Данные отсортированы!\n");
    }

    static void ConvertCurrency(string[] names, decimal[] costs, int n)
    {
        Console.WriteLine("Выберите в какую валюту хотите перевести: ");
        Console.WriteLine("1. Доллар");
        Console.WriteLine("2. Евро");
        Console.WriteLine("3. Дирхам");
        Console.WriteLine("4. Своя валюта");


        decimal[] convertedCosts = new decimal[n];
        for (int i = 0; i < n; i++)
        {
            convertedCosts[i] = costs[i];
        }

        int vibor = Convert.ToInt32(Console.ReadLine());
        string currencyName = "";
        decimal usd = 83.0m;
        decimal eur = 97.0m;
        decimal dirham = 22.56m;
        decimal customRate;



        if (vibor == 1)
        {
            currencyName = "доллар";
            for (int i = 0; i < n; i++)
            {
                convertedCosts[i] = convertedCosts[i] / usd;
                Console.WriteLine($"{names[i]} - {convertedCosts[i]} usd"); // ДОДЕЛАТЬ
            }
        }
        if (vibor == 2)
        {
            currencyName = "евро";
            for (int i = 0; i < n; i++)
            {
                convertedCosts[i] = convertedCosts[i] / eur;
                Console.WriteLine($"{names[i]} - {convertedCosts[i]} eur");
            }
        }
        if (vibor == 3)
        {
            currencyName = "дирхам";
            for (int i = 0; i < n; i++)
            {
                convertedCosts[i] = convertedCosts[i] / dirham;
                Console.WriteLine($"{names[i]} - {convertedCosts[i]} dirham");
            }
        }
        if (vibor == 4)
        {
            Console.Write("Введите курс (1 рубль = ?): ");
            customRate = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Введите название валютыы: ");
            currencyName = Console.ReadLine();

            for (int i = 0; i < n; i++)
            {
                convertedCosts[i] = convertedCosts[i] / customRate;
                Console.WriteLine($"{names[i]} - {convertedCosts[i]} {currencyName}");
            }
        }
    }
    static void SearchName(string[] names, decimal[] costs, int n)
        {
            Console.Write("Введите название для поиска: ");
            string search = Console.ReadLine().ToLower();

            Console.WriteLine("\nРезультаты поиска:");
            bool found = false;
            for (int i = 0; i < n; i++)
            {
                if (names[i].ToLower().Contains(search))
                {
                    Console.WriteLine($"{names[i]} - {costs[i]} руб.");
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Не найдено!");
            Console.WriteLine();
        
    }
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
                    ShowData(names, costs, n);
                    break;
                case "2":
                    ShowStats(costs, n);
                    break;
                case "3":
                    SortCosts(names, costs, n);
                    ShowData(names, costs, n);
                    break;
                case "4":
                    ConvertCurrency(names, costs, n);
                    break;
                case "5":
                    SearchName(names, costs, n);
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