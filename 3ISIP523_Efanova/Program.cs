using System;
using System.Globalization;

namespace StoreApp
{
    enum Category
    {
        Food, Pets, Clothes
    }
    class Product
    {
        private static int nextID = 1;

        public int Code;
        public string Name;
        public double Prise;
        public int Quantity;
        public bool InStock => Quantity > 0;
        public Category Category;

        public Product(string name, double price, int quantity, Category category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым");
            if (price <= 0)
                throw new ArgumentException("Цена должна быть больше нуля");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");

            Code = nextID++;
            Name = name;
            Prise = price;
            Quantity = quantity;
            Category = category;

        }

        public void PrintInfo()
        {
            Console.WriteLine($"Код: {Code} | Название: {Name} | Цена: {Prise} руб. | " + $"Количество: {Quantity} | В наличии: {(InStock ? "ДА" : "НЕТ")} | Категория: {Category}");
        }

        public void AddStock(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Количество не может быть отрицательным");
            Quantity += amount;
        }
        public void Sell(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Количество должно быть положительным");
            if (Quantity < amount)
                throw new ArgumentException("Недостаточно товара на складе");
            Quantity -= amount;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();

            try
            {
                products.Add(new Product("Хлеб", 45.5, 20, Category.Food));
                products.Add(new Product("Молоко 1л", 80, 10, Category.Food));
                products.Add(new Product("Ошейник", 1500, 5, Category.Pets));
                products.Add(new Product("Футболка", 700, 15, Category.Clothes));
                products.Add(new Product("Собака\"", 45000, 2, Category.Pets));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании тестовых данных: " + ex.Message);
            }

            while (true)
            {
                Console.WriteLine("\n=== Учет товаров в магазине ===");
                Console.WriteLine("1 - Добавить товар");
                Console.WriteLine("2 - Удалить товар");
                Console.WriteLine("3 - Заказать поставку (пополнить запас)");
                Console.WriteLine("4 - Продать товар");
                Console.WriteLine("5 - Поиск товара");
                Console.WriteLine("6 - Показать все товары");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine()?.Trim();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddProduct(products);
                            break;
                        case "2":
                            DeleteProduct(products);
                            break;
                        case "3":
                            OrderSupply(products);
                            break;
                        case "4":
                            SellProduct(products);
                            break;
                        case "5":
                            SearchProducts(products);
                            break;
                        case "6":
                            ShowAll(products);
                            break;
                        case "0":
                            Console.WriteLine("Выход...");
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Введите цифру команды.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                }

            }
            static string ReadNonEmptyString(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                        return input.Trim();
                    Console.WriteLine("Значение не может быть пустым. Попробуйте снова.");
                }
            }

            static double ReadPositiveDouble(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string raw = Console.ReadLine()?.Trim() ?? "";
                    raw = raw.Replace(',', '.');
                    if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out double val) && val > 0)
                        return val;
                    Console.WriteLine("Введите корректное положительное число (например: 123.45).");
                }
            }

            static int ReadNonNegativeInt(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string raw = Console.ReadLine()?.Trim() ?? "";
                    if (int.TryParse(raw, out int val) && val >= 0)
                        return val;
                    Console.WriteLine("Введите корректное целое число, больше либо равно 0.");
                }
            }

            static int ReadPositiveInt(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string raw = Console.ReadLine()?.Trim() ?? "";
                    if (int.TryParse(raw, out int val) && val > 0)
                        return val;
                    Console.WriteLine("Введите корректное целое положительное число (> 0).");
                }
            }

            static Category ReadCategory()
            {
                while (true)
                {
                    Console.WriteLine("Выберите категорию:");
                    foreach (var c in Enum.GetValues(typeof(Category)))
                    {
                        Console.WriteLine($"{(int)c} - {c}");
                    }
                    Console.Write("Введите номер категории: ");
                    string raw = Console.ReadLine()?.Trim() ?? "";
                    if (int.TryParse(raw, out int idx) && Enum.IsDefined(typeof(Category), idx))
                    {
                        return (Category)idx;
                    }
                    Console.WriteLine("Неверный выбор категории. Попробуйте снова.");
                }
            }

        }
    }
}