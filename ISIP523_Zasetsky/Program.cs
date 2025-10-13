using System;
using System.Collections.Generic;
using System.Linq;

public enum Genre
{
    Fantasy,
    ScienceFiction,
    Mystery,
    Romance,
    Thriller,
    Biography,
    History
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Genre Genre { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Название: {Title}, Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
    }
}
class Program
{
    static List<Book> books = new List<Book>();
    static int nextId = 1;

    static void Main(string[] args)
    {
        InitializeTestData();

        Console.WriteLine("=== СИСТЕМА УЧЁТА БИБЛИОТЕКИ ===");

        while (true)
        {
            ShowMenu();

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Ошибка ввода! Введите число от 1 до 8.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddBook();
                    break;
                case 2:
                    RemoveBook();
                    break;
                case 3:
                    SearchBooks();
                    break;
                case 4:
                    SortBooks();
                    break;
                case 5:
                    ShowPriceExtremes();
                    break;
                case 6:
                    ShowBooksByAuthors();
                    break;
                case 7:
                    ShowAllBooks();
                    break;
                case 8:
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор! Введите число от 1 до 8.");
                    break;
            }
        }
    }

    static void InitializeTestData()
    {
        books.AddRange(new[]
        {
            new Book { Id = nextId++, Title = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", Genre = Genre.Fantasy, Year = 1997, Price = 5000 },
            new Book { Id = nextId++, Title = "1984", Author = "Джордж Оруэлл", Genre = Genre.ScienceFiction, Year = 1949, Price = 800 },
            new Book { Id = nextId++, Title = "Убийство в Восточном экспрессе", Author = "Агата Кристи", Genre = Genre.Mystery, Year = 1934, Price = 700 },
            new Book { Id = nextId++, Title = "Евгений Онегин", Author = "А. С. Пушкин", Genre = Genre.Romance, Year = 1823, Price = 1000 },
            new Book { Id = nextId++, Title = "Шерлок Холмс", Author = "Артур Конан Дойл", Genre = Genre.Mystery, Year = 1887, Price = 900 }
        });

        Console.WriteLine("Добавлено 5 тестовых книг.");
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
        Console.WriteLine("1. Добавить книгу");
        Console.WriteLine("2. Удалить книгу по ID");
        Console.WriteLine("3. Найти книги");
        Console.WriteLine("4. Отсортировать книги");
        Console.WriteLine("5. Самая дорогая и дешёвая книга");
        Console.WriteLine("6. Книги по авторам");
        Console.WriteLine("7. Показать все книги");
        Console.WriteLine("8. Выход");
        Console.Write("Выберите действие: ");
    }

    static void AddBook()
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОЙ КНИГИ ===");

        Console.Write("Название: ");
        string title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Ошибка: название не может быть пустым!");
            return;
        }

        Console.Write("Автор: ");
        string author = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(author))
        {
            Console.WriteLine("Ошибка: автор не может быть пустым!");
            return;
        }


        Console.WriteLine("Выберите жанр:");
        var genres = Enum.GetValues(typeof(Genre));
        for (int i = 0; i < genres.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
        }

        Console.Write("Жанр (номер): ");
        if (!int.TryParse(Console.ReadLine(), out int genreChoice) || genreChoice < 1 || genreChoice > genres.Length)
        {
            Console.WriteLine($"Ошибка: выберите жанр от 1 до {genres.Length}!");
            return;
        }

        Console.Write("Год издания: ");
        if (!int.TryParse(Console.ReadLine(), out int year) || year < 1000 || year > DateTime.Now.Year)
        {
            Console.WriteLine($"Ошибка: год должен быть от 1000 до {DateTime.Now.Year}!");
            return;
        }

        Console.Write("Цена: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
        {
            Console.WriteLine("Ошибка: цена должна быть положительным числом!");
            return;
        }

        var newBook = new Book
        {
            Id = nextId++,
            Title = title.Trim(),
            Author = author.Trim(),
            Genre = (Genre)(genreChoice - 1),
            Year = year,
            Price = price
        };

        books.Add(newBook);
        Console.WriteLine($"Книга '{title}' успешно добавлена с ID {newBook.Id}!");
    }
    static void RemoveBook()
    {
        if (!books.Any())
        {
            Console.WriteLine("Библиотека пуста!");
            return;
        }

        Console.WriteLine("\n=== УДАЛЕНИЕ КНИГИ ===");
        ShowAllBooks();

        Console.Write("Введите ID книги для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int idToRemove))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }

        var bookToRemove = books.FirstOrDefault(b => b.Id == idToRemove);

        if (bookToRemove != null)
        {
            books.Remove(bookToRemove);
            Console.WriteLine($"Книга '{bookToRemove.Title}' успешно удалена!");
        }
        else
        {
            Console.WriteLine("Книга с таким ID не найдена!");
        }
    }

    static void SearchBooks()
    {
        if (!books.Any())
        {
            Console.WriteLine("Библиотека пуста!");
            return;
        }

        Console.WriteLine("\n=== ПОИСК КНИГ ===");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По автору");
        Console.WriteLine("3. По жанру");
        Console.Write("Выберите тип поиска: ");

        if (!int.TryParse(Console.ReadLine(), out int searchType) || searchType < 1 || searchType > 3)
        {
            Console.WriteLine("Ошибка: выберите вариант от 1 до 3!");
            return;
        }

        List<Book> searchResults;


        switch (searchType)
        {
            case 1:
                Console.Write("Введите название для поиска: ");
                string titleSearch = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(titleSearch))
                {
                    Console.WriteLine("Поисковый запрос не может быть пустым!");
                    return;
                }
                searchResults = books
                    .Where(b => b.Title.ToLower().Contains(titleSearch.ToLower()))
                    .ToList();
                break;
            case 2:
                Console.Write("Введите автора для поиска: ");
                string authorSearch = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(authorSearch))
                {
                    Console.WriteLine("Поисковый запрос не может быть пустым!");
                    return;
                }
                searchResults = books
                    .Where(b => b.Author.ToLower().Contains(authorSearch.ToLower()))
                    .ToList();
                break;
            case 3:
                Console.WriteLine("Выберите жанр:");
                var genres = Enum.GetValues(typeof(Genre));
                for (int i = 0; i < genres.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
                }
                Console.Write("Жанр (номер): ");
                if (!int.TryParse(Console.ReadLine(), out int genreChoice) || genreChoice < 1 || genreChoice > genres.Length)
                {
                    Console.WriteLine($"Ошибка: выберите жанр от 1 до {genres.Length}!");
                    return;
                }
                Genre selectedGenre = (Genre)(genreChoice - 1);
                searchResults = books
                    .Where(b => b.Genre == selectedGenre)
                    .ToList();
                break;
            default:
                return;
        }

        if (searchResults.Count > 0)
        {
            Console.WriteLine($"\nНайдено книг: {searchResults.Count}");
            Console.WriteLine(new string('-', 80));
            foreach (var book in searchResults)
            {
                Console.WriteLine(book);
            }
        }
        else
        {
            Console.WriteLine("Книги по вашему запросу не найдены.");
        }
    }

    static void SortBooks()
    {
        if (!books.Any())
        {
            Console.WriteLine("Библиотека пуста!");
            return;
        }

        Console.WriteLine("\n=== СОРТИРОВКА КНИГ ===");
        Console.WriteLine("1. По названию (А-Я)");
        Console.WriteLine("2. По названию (Я-А)");
        Console.WriteLine("3. По году (сначала старые)");
        Console.WriteLine("4. По году (сначала новые)");
        Console.WriteLine("5. По цене (сначала дешёвые)");
        Console.WriteLine("6. По цене (сначала дорогие)");
        Console.Write("Выберите тип сортировки: ");

        if (!int.TryParse(Console.ReadLine(), out int sortType) || sortType < 1 || sortType > 6)
        {
            Console.WriteLine("Ошибка: выберите вариант от 1 до 6!");
            return;
        }

        List<Book> sortedBooks;


        switch (sortType)
        {
            case 1:
                sortedBooks = books
                    .OrderBy(b => b.Title)
                    .ToList();
                break;
            case 2:
                sortedBooks = books
                    .OrderByDescending(b => b.Title)
                    .ToList();
                break;
            case 3:
                sortedBooks = books
                    .OrderBy(b => b.Year)
                    .ToList();
                break;
            case 4:
                sortedBooks = books
                    .OrderByDescending(b => b.Year)
                    .ToList();
                break;
            case 5:
                sortedBooks = books
                    .OrderBy(b => b.Price)
                    .ToList();
                break;
            case 6:
                sortedBooks = books
                    .OrderByDescending(b => b.Price)
                    .ToList();
                break;
            default:
                return;
        }

        Console.WriteLine("\nОтсортированные книги:");
        Console.WriteLine(new string('-', 80));
        foreach (var book in sortedBooks)
        {
            Console.WriteLine(book);
        }
    }
    static void ShowPriceExtremes()
    {
        if (!books.Any())
        {
            Console.WriteLine("Библиотека пуста!");
            return;
        }

        Console.WriteLine("\n=== САМАЯ ДОРОГАЯ И ДЕШЁВАЯ КНИГА ===");

        List<Book> expensiveBooks = books
            .OrderByDescending(b => b.Price)
            .Take(1)
            .ToList();

        List<Book> cheapBooks = books
            .OrderBy(b => b.Price)
            .Take(1)
            .ToList();

        if (expensiveBooks.Count > 0)
        {
            Console.WriteLine($"Самая дорогая книга: {expensiveBooks[0]}");
        }
        if (cheapBooks.Count > 0)
        {
            Console.WriteLine($"Самая дешёвая книга: {cheapBooks[0]}");
        }
    }

    static void ShowBooksByAuthors()
    {
        if (!books.Any())
        {
            Console.WriteLine("Библиотека пуста!");
            return;
        }

        Console.WriteLine("\n=== КНИГИ ПО АВТОРАМ ===");

        List<IGrouping<string, Book>> booksByAuthor = books
            .GroupBy(b => b.Author)
            .OrderBy(g => g.Key)
            .ToList();

        foreach (var authorGroup in booksByAuthor)
        {
            List<Book> authorBooks = authorGroup
                .OrderBy(b => b.Year)
                .ToList();

            Console.WriteLine($"\nАвтор: {authorGroup.Key}");
            Console.WriteLine($"Количество книг: {authorBooks.Count}");
            Console.WriteLine("Книги:");
            foreach (var book in authorBooks)
            {
                Console.WriteLine($"  - {book.Title} ({book.Year}) - {book.Genre} - {book.Price:C}");
            }
            decimal totalValue = authorBooks.Sum(b => b.Price);
            Console.WriteLine($"Общая стоимость: {totalValue:C}");
        }
    }

    static void ShowAllBooks()
    {
        if (!books.Any())
        {
            Console.WriteLine("Библиотека пуста!");
            return;
        }

        Console.WriteLine("\n=== ВСЕ КНИГИ В БИБЛИОТЕКЕ ===");
        Console.WriteLine($"Всего книг: {books.Count}");
        decimal totalValue = books.Sum(b => b.Price);
        Console.WriteLine($"Общая стоимость коллекции: {totalValue:C}");
        Console.WriteLine(new string('-', 80));

        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }

