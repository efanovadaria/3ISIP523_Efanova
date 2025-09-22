using System;

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
    }
}