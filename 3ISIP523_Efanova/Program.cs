using System;
using System.Collections.Generic;
using System.Text;

namespace TextStatisticsApp
{
    class Program
    {
        class TextStats
        {
            public int WordCount;
            public string ShortestWord;
            public string LongestWord;
            public int SentenceCount;
            public int VowelCount;
            public int ConsonantCount;
            public Dictionary<char, int> LetterFrequency;
        }

        static void Main(string[] args)
        {
            List<TextStats> allStats = new List<TextStats>(); 
            bool continueProgram = true;

            while (continueProgram)
            {
                Console.Clear();
                Console.WriteLine("Введите текст (минимум 100 символов):");
                string text = Console.ReadLine();

                while (text.Length < 100)
                {
                    Console.WriteLine("Ошибка! Текст должен содержать минимум 100 символов. Попробуйте снова:");
                    text = Console.ReadLine();
                }

                TextStats stats = AnalyzeText(text);
                allStats.Add(stats);

                DisplayStats(stats);

                Console.WriteLine("\nХотите ввести новый текст? (д/н)");
                string answer = Console.ReadLine().ToLower();

                if (answer == "н")
                {
                    continueProgram = false;
                }
            }

            Console.WriteLine("\n======= История всех анализов =======");
            int index = 1;
            foreach (var stat in allStats)
            {
                Console.WriteLine($"\n--- Анализ №{index} ---");
                DisplayStats(stat);
                index++;
            }

            Console.WriteLine("\nРабота программы завершена.");
        }

        static TextStats AnalyzeText(string text)
        {
            TextStats stats = new TextStats();
            stats.LetterFrequency = new Dictionary<char, int>();

            string[] words = SplitWords(text);
            stats.WordCount = words.Length;

            stats.ShortestWord = FindShortestWord(words);
            stats.LongestWord = FindLongestWord(words);
            stats.SentenceCount = CountSentences(text);

            CountVowelsAndConsonants(text, out stats.VowelCount, out stats.ConsonantCount);
            stats.LetterFrequency = CountLetterFrequency(text);

            return stats;
        }

        static string[] SplitWords(string text)
        {
            char[] separators = { ' ', '\n', '\r', '\t', ',', '.', '!', '?', ';', ':', '-', '(', ')', '"' };
            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }

        static string FindShortestWord(string[] words)
        {
            if (words.Length == 0) return "";
            string shortest = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < shortest.Length)
                    shortest = words[i];
            }
            return shortest;
        }

        static string FindLongestWord(string[] words)
        {
            if (words.Length == 0) return "";
            string longest = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length > longest.Length)
                    longest = words[i];
            }
            return longest;
        }

        // Подсчёт количества предложений
        static int CountSentences(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (c == '.' || c == '!' || c == '?')
                    count++;
            }
            return count;
        }

        // Подсчёт количества гласных и согласных
        static void CountVowelsAndConsonants(string text, out int vowels, out int consonants)
        {
            vowels = 0;
            consonants = 0;
            string vowelChars = "аеёиоуыэюяaeiou";
            string consonantChars = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";

            text = text.ToLower();

            foreach (char c in text)
            {
                if (vowelChars.IndexOf(c) >= 0)
                    vowels++;
                else if (consonantChars.IndexOf(c) >= 0)
                    consonants++;
            }
        }

        // Подсчёт частоты каждой буквы
        static Dictionary<char, int> CountLetterFrequency(string text)
        {
            Dictionary<char, int> freq = new Dictionary<char, int>();
            text = text.ToLower();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    if (freq.ContainsKey(c))
                        freq[c]++;
                    else
                        freq[c] = 1;
                }
            }

            return freq;
        }

        // Вывод статистики на экран
        static void DisplayStats(TextStats stats)
        {
            Console.WriteLine("\n=== Результаты анализа ===");
            Console.WriteLine($"Количество слов: {stats.WordCount}");
            Console.WriteLine($"Самое короткое слово: {stats.ShortestWord}");
            Console.WriteLine($"Самое длинное слово: {stats.LongestWord}");
            Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
            Console.WriteLine($"Количество гласных: {stats.VowelCount}");
            Console.WriteLine($"Количество согласных: {stats.ConsonantCount}");
            Console.WriteLine("Частота букв:");

            foreach (var kvp in stats.LetterFrequency)
            {
                Console.WriteLine($"   {kvp.Key}: {kvp.Value}");
            }
        }
    }
}
