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

    }
}