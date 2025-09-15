using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int n;
        do
        {
            Console.Write("Введите количество операций (от 2 до 40): ");
        } while (!int.TryParse(Console.ReadLine(), out n) || n < 2 || n > 40);

        string[] names = new string[n];
        double[] prices = new double[n];

        Console.WriteLine("\nВведите траты в формате: Название; Сумма");
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Операция {i + 1}: ");
            string input = Console.ReadLine();
            string[] parts = input.Split(';');
            if (parts.Length != 2 || !double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out prices[i]))
            {
                Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                i--;
                continue;
            }
            names[i] = parts[0].Trim();
        }

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nВаши траты:");
                    for (int i = 0; i < n; i++)
                        Console.WriteLine($"{names[i]}: {prices[i]} руб.");
                    break;

                case "2": 
                    double sum = 0, min = prices[0], max = prices[0];
                    foreach (double p in prices)
                    {
                        sum += p;
                        if (p < min) min = p;
                        if (p > max) max = p;
                    }
                    double avg = sum / n;
                    Console.WriteLine($"\nСтатистика:");
                    Console.WriteLine($"Сумма: {sum} руб.");
                    Console.WriteLine($"Среднее: {avg:F2} руб.");
                    Console.WriteLine($"Минимум: {min} руб.");
                    Console.WriteLine($"Максимум: {max} руб.");
                    break;

                case "3": 
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < n - i - 1; j++)
                        {
                            if (prices[j] > prices[j + 1])
                            {
                                double tempPrice = prices[j];
                                prices[j] = prices[j + 1];
                                prices[j + 1] = tempPrice;

                                string tempName = names[j];
                                names[j] = names[j + 1];
                                names[j + 1] = tempName;
                            }
                        }
                    }
                    Console.WriteLine("\nДанные отсортированы по цене.");
                    break;

                case "4": 
                    Console.Write("Введите курс для конвертации (курс рубля к доллару): ");
                    if (double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double rate) && rate > 0)
                    {
                        Console.WriteLine("\nСуммы в новой валюте:");
                        for (int i = 0; i < n; i++)
                            Console.WriteLine($"{names[i]}: {(prices[i] / rate):F2}");
                    }
                    else
                    {
                        Console.WriteLine("Некорректный курс.");
                    }
                    break;

            }
        }
    }
}