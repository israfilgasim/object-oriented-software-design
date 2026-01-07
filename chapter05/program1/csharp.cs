using System;

namespace ObjectOrientedDesign
{
    public class Item
    {
        // 1. Auto-implemented properties for read-only data.
        // "get; private set;" means anyone can read it, but only this class can set it initially.
        public string Name { get; }
        public double Weight { get; }

        // Backing field for Price so we can add logic to the setter
        private double _price;

        // 2. Full Property with Validation Logic
        public double Price
        {
            get => _price; // Expression-bodied member (shorthand for { return _price; })
            set
            {
                // Idiomatic C#: Throw an exception for invalid arguments rather than asserting.
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Price), "Price must be greater than 0.");
                }
                _price = value;
            }
        }

        // Constructor
        public Item(string name, double weight, double price)
        {
            Name = name;
            Weight = weight;
            // Set via property to ensure validation logic runs even during construction
            Price = price;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var item = new Item("whole chicken", 4.5, 10.31);

            // Idiomatic C#: String Interpolation ($"...")
            Console.WriteLine($"  name: {item.Name}");
            Console.WriteLine($"weight: {item.Weight} lbs");
            Console.WriteLine($" price: {item.Price:C}"); // :C formats as Currency automatically

            try
            {
                // We assign to the property directly
                item.Price = -9.99;
                Console.WriteLine($"\nnew price: {item.Price:C}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Handle the error gracefully
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }
    }
}