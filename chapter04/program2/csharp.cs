using System;
using System.Collections.Generic; // Required for List<T>

namespace AutomobileSimulation
{
    // Type aliases
    using Brake = System.Int32;
    using CarWax = System.Int32;
    using Direction = System.Int32;
    using Engine = System.Int32;
    using Headlight = System.Int32;
    using Oil = System.Int32;
    using Soap = System.Int32;
    using Tire = System.Int32;
    using Vacuum = System.Int32;

    // 1. The Entity: Automobile
    public class Automobile
    {
        // Private State
        private Brake[] brakes = new Brake[4];
        private Direction heading;
        private Headlight[] headlights = new Headlight[4];
        private int speed;
        private Tire[] tires = new Tire[4];

        // Public Behaviors (Driving only)
        public void Accelerate() { /* ... */ }
        public void ApplyBrakes() { /* ... */ }
        public void ShutOffEngine() { /* ... */ }
        public void StartEngine() { /* ... */ }
        public void TurnLeft() { /* ... */ }
        public void TurnRight() { /* ... */ }
    }

    // 2. The Service: Garage
    public class Garage
    {
        private Automobile car; // Reference to the car
        private Oil newOil;
        private List<Tire> newTires; // vector<T> becomes List<T>

        // Constructor via Dependency Injection
        public Garage(Automobile c)
        {
            this.car = c;
            this.newTires = new List<Tire>();
        }

        public void AdjustHeadlights() { /* ... */ }
        public void ChangeOil() { /* ... */ }
        public void ChangeTires() { /* ... */ }
        public void CheckBrakes() { /* ... */ }
        public void CheckTires() { /* ... */ }
        public void RotateTires() { /* ... */ }
        public void TuneupEngine() { /* ... */ }
    }

    // 3. The Service: CarWash
    public class CarWash
    {
        private Automobile car; // Reference to the car
        private Soap soap;
        private Vacuum vacuumCleaner;
        private CarWax wax;

        public CarWash(Automobile c)
        {
            this.car = c;
        }

        public void VacuumCar() { /* ... */ }
        public void WashCar() { /* ... */ }
        public void WaxCar() { /* ... */ }
    }

    // Tester / Main
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate the Car
            Automobile myCar = new Automobile();

            // Pass the Car to the services
            Garage myGarage = new Garage(myCar);
            CarWash myWash = new CarWash(myCar);

            Console.WriteLine("Automobile, Garage, and CarWash initialized.");
        }
    }
}