using System;

namespace VehicleSimulation
{
    // Type Aliases (Global using in C# 10+)
    using Brake = System.Int32;
    using Direction = System.Int32;
    using Engine = System.Int32;

    // 1. The Interface
    // C# convention uses an "I" prefix for interfaces.
    // This defines the Contract.
    public interface IMotorVehicle
    {
        void StartEngine();
        void StopEngine();
        void Accelerate();
        void TurnLeft();
        void TurnRight();
        void ApplyBrakes();
        void Drive();
    }

    // 2. The Car Implementation
    public class Car : IMotorVehicle
    {
        // Private Member Variables (State)
        private Brake[] brakes = new Brake[4];
        private Engine engine;
        private Direction heading;
        private int speed;

        // Interface Implementation
        // Note: These MUST be public to satisfy the interface in C#
        public void StartEngine() => Console.WriteLine("car starts engine");
        public void StopEngine() => Console.WriteLine("car stops engine");
        public void Accelerate() => Console.WriteLine("car accelerates");
        public void TurnLeft() => Console.WriteLine("car turns left");
        public void TurnRight() => Console.WriteLine("car turns right");
        public void ApplyBrakes() => Console.WriteLine("car applies brakes");

        public void Drive()
        {
            StartEngine();
            Accelerate();
            TurnLeft();
            TurnRight();
            ApplyBrakes();
            StopEngine();
            Console.WriteLine();
        }
    }

    // 3. The Truck Implementation
    public class Truck : IMotorVehicle
    {
        // Private State
        private Brake[] brakes = new Brake[4];
        private Engine engine;
        private Direction heading;
        private int speed;

        // Interface Implementation
        public void StartEngine() => Console.WriteLine("truck starts engine");
        public void StopEngine() => Console.WriteLine("truck stops engine");
        public void Accelerate() => Console.WriteLine("truck accelerates");
        public void TurnLeft() => Console.WriteLine("truck turns left");
        public void TurnRight() => Console.WriteLine("truck turns right");
        public void ApplyBrakes() => Console.WriteLine("truck applies brakes");

        public void Drive()
        {
            StartEngine();
            Accelerate();
            TurnLeft();
            TurnRight();
            ApplyBrakes();
            StopEngine();
            Console.WriteLine();
        }
    }

    // 4. Tester
    class Program
    {
        static void Main(string[] args)
        {
            // We can treat them as their concrete types...
            Car car = new Car();
            car.Drive();

            // ...or we could treat them as the Interface type (Polymorphism)
            IMotorVehicle truck = new Truck();
            truck.Drive();
        }
    }
}