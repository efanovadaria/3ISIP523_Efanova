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

        public int Code { get; private set; }
        public string Name { get; private set; }
        public double Prise { get; private set; }
        public int Quantity { get; private set; }
        public bool InStock => Quantity > 0;
        public Category Category { get; private set; }

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
    }
}