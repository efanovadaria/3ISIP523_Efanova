using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LibraryApp
{
    public enum Genre
    {
        Fiction = 1,
        Fantasy,
        Science,
        History,
        Romance
    }

    public class Book
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public Genre Genre { get; }
        public int Year { get; }
        public decimal Price { get; }

        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название не может быть пустым.");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор не может быть пустым.");
            if (year <= 0 || year > DateTime.Now.Year)
                throw new ArgumentException($"Год издания должен быть от 1 до {DateTime.Now.Year}.");
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной.");

            Id = _nextId++;
            Title = title.Trim();
            Author = author.Trim();
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {Id} | \"{Title}\" | Автор: {Author} | Жанр: {Genre} | Год: {Year} | Цена: {Price:N2} руб.";
        }
    }

    class Program
    {
        static List<Book> books = new List<Book>
        {
            new Book("Мастер и Маргарита", "М. Булгаков", Genre.Fiction, 1966, 500m),
            new Book("Война и мир", "Л. Толстой", Genre.History, 1869, 700m),
            new Book("Гарри Поттер", "Дж. Роулинг", Genre.Fantasy, 1997, 450m),
            new Book("1984", "Дж. Оруэлл", Genre.Science, 1949, 600m),
            new Book("Преступление и наказание", "Ф. Достоевский", Genre.Fiction, 1866, 550m)
        };

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Учёт книг в библиотеке ===");

            while (true)
            {
                ShowMenu();
                Console.Write("\nВведите команду: ");
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": AddBook(); break;
                    case "2": RemoveBook(); break;
                    case "3": SearchBooks(); break;
                    case "4": SortBooks(); break;
                    case "5": ShowExtremePrices(); break;
                    case "6": GroupByAuthor(); break;
                    case "7": ShowAllBooks(); break;
                    case "0":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Некорректный выбор. Повторите ввод.");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1 - Добавить книгу");
            Console.WriteLine("2 - Удалить книгу по ID");
            Console.WriteLine("3 - Найти книгу (по названию, автору, жанру)");
            Console.WriteLine("4 - Отсортировать книги (по названию или году)");
            Console.WriteLine("5 - Самая дорогая и дешёвая книга");
            Console.WriteLine("6 - Сгруппировать книги по авторам");
            Console.WriteLine("7 - Показать все книги");
            Console.WriteLine("0 - Выход");
        }

        // === Добавление книги ===
        static void AddBook()
        {
            Console.WriteLine("\n--- Добавление книги ---");
            string title = ReadNonEmpty("Введите название: ");
            string author = ReadNonEmpty("Введите автора: ");
            Genre genre = ReadGenre();
            int year = ReadInt("Введите год издания: ", 1, DateTime.Now.Year);
            decimal price = ReadDecimal("Введите цену (руб.): ", 0, 1_000_000);

            try
            {
                var book = new Book(title, author, genre, year, price);
                books.Add(book);
                Console.WriteLine("Книга добавлена: " + book);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        // === Удаление книги ===
        static void RemoveBook()
        {
            Console.WriteLine("\n--- Удаление книги ---");
            if (!books.Any())
            {
                Console.WriteLine("Нет книг для удаления.");
                return;
            }

            int id = ReadInt("Введите ID книги: ", 1);
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                Console.WriteLine("Книга с таким ID не найдена.");
            }
            else
            {
                books.Remove(book);
                Console.WriteLine($"Книга \"{book.Title}\" удалена.");
            }
        }

        // === Поиск книг ===
        static void SearchBooks()
        {
            Console.WriteLine("\n--- Поиск книг ---");
            Console.WriteLine("1 - По названию");
            Console.WriteLine("2 - По автору");
            Console.WriteLine("3 - По жанру");
            Console.Write("Выберите тип поиска: ");
            string type = Console.ReadLine()?.Trim();

            IEnumerable<Book> found = Enumerable.Empty<Book>();

            switch (type)
            {
                case "1":
                    string title = ReadNonEmpty("Введите название или его часть: ");
                    found = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
                    break;
                case "2":
                    string author = ReadNonEmpty("Введите имя автора или его часть: ");
                    found = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
                    break;
                case "3":
                    Genre genre = ReadGenre();
                    found = books.Where(b => b.Genre == genre);
                    break;
                default:
                    Console.WriteLine("Некорректный выбор.");
                    return;
            }

            PrintBooks(found);
        }

        // === Сортировка книг ===
        static void SortBooks()
        {
            Console.WriteLine("\n--- Сортировка книг ---");
            Console.WriteLine("1 - По названию");
            Console.WriteLine("2 - По году");
            Console.Write("Ваш выбор: ");
            string sortType = Console.ReadLine()?.Trim();

            IEnumerable<Book> sorted = sortType switch
            {
                "1" => books.OrderBy(b => b.Title),
                "2" => books.OrderBy(b => b.Year),
                _ => Enumerable.Empty<Book>()
            };

            PrintBooks(sorted);
        }

        // === Самая дорогая и дешевая книга ===
        static void ShowExtremePrices()
        {
            Console.WriteLine("\n--- Самая дорогая и самая дешёвая книга ---");
            if (!books.Any())
            {
                Console.WriteLine("Список пуст.");
                return;
            }

            var minPrice = books.Min(b => b.Price);
            var maxPrice = books.Max(b => b.Price);

            Console.WriteLine($"\nСамая дешёвая книга ({minPrice:N2} руб.):");
            PrintBooks(books.Where(b => b.Price == minPrice));

            Console.WriteLine($"\nСамая дорогая книга ({maxPrice:N2} руб.):");
            PrintBooks(books.Where(b => b.Price == maxPrice));
        }

        // === Группировка по авторам ===
        static void GroupByAuthor()
        {
            Console.WriteLine("\n--- Группировка книг по авторам ---");
            var groups = books.GroupBy(b => b.Author)
                              .Select(g => new { Author = g.Key, Count = g.Count() })
                              .OrderByDescending(g => g.Count);

            foreach (var g in groups)
                Console.WriteLine($"{g.Author}: {g.Count} книг(и)");
        }

        // === Показать все книги ===
        static void ShowAllBooks()
        {
            Console.WriteLine("\n--- Все книги ---");
            PrintBooks(books);
        }

        // === Вспомогательные методы ===
        static void PrintBooks(IEnumerable<Book> list)
        {
            var data = list.ToList();
            if (!data.Any())
            {
                Console.WriteLine("Ничего не найдено.");
                return;
            }

            foreach (var book in data)
                Console.WriteLine(book);
        }

        static string ReadNonEmpty(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                Console.WriteLine("Ошибка: поле не может быть пустым.");
            }
        }

        static int ReadInt(string msg, int min, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(msg);
                if (int.TryParse(Console.ReadLine(), out int val) && val >= min && val <= max)
                    return val;
                Console.WriteLine("Ошибка ввода числа.");
            }
        }

        static decimal ReadDecimal(string msg, decimal min, decimal max)
        {
            while (true)
            {
                Console.Write(msg);
                string s = Console.ReadLine()?.Replace(',', '.');
                if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal val)
                    && val >= min && val <= max)
                    return val;
                Console.WriteLine("Ошибка: введите корректное число.");
            }
        }

        static Genre ReadGenre()
        {
            Console.WriteLine("Выберите жанр:");
            foreach (var g in Enum.GetValues(typeof(Genre)))
                Console.WriteLine($"{(int)g} - {g}");
            int val = ReadInt("Введите номер жанра: ", 1, Enum.GetValues(typeof(Genre)).Length);
            return (Genre)val;
        }
    }
}

