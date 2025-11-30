using System;

namespace VehicleSimulation
{
    // 1. The Abstract Base Class
    // Equivalent to MotorVehicle.h
    public abstract class MotorVehicle
    {
        // "abstract" in C# = "pure virtual" in C++ (= 0)
        // These MUST be implemented by subclasses.
        protected abstract void StartEngine();
        protected abstract void StopEngine();

        // "virtual" allows subclasses to override if they want,
        // but provides a default implementation.
        protected virtual void Accelerate()
        {
            Console.WriteLine("vehicle accelerates");
        }

        protected virtual void TurnLeft()
        {
            Console.WriteLine("vehicle turns left");
        }

        protected virtual void TurnRight()
        {
            Console.WriteLine("vehicle turns right");
        }

        protected virtual void ApplyBrakes()
        {
            Console.WriteLine("vehicle applies brakes");
        }

        // The "Template Method". It is public so we can call it.
        // It orchestrates the other methods.
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

    // 2. The Car Class
    // Equivalent to Car.h
    public class Car : MotorVehicle
    {
        // 'override' is mandatory in C# to change behavior
        protected override void StartEngine()
        {
            Console.WriteLine("car starts engine");
        }

        protected override void StopEngine()
        {
            Console.WriteLine("car stops engine");
        }

        // Car does NOT override TurnLeft/Right, so it uses the Base logic.
    }

    // 3. The Truck Class
    // Equivalent to Truck.h
    public class Truck : MotorVehicle
    {
        protected override void StartEngine()
        {
            Console.WriteLine("truck starts engine");
        }

        protected override void StopEngine()
        {
            Console.WriteLine("truck stops engine");
        }

        // Truck DOES override turns (unlike Car)
        protected override void TurnLeft()
        {
            Console.WriteLine("truck turns left");
        }

        protected override void TurnRight()
        {
            Console.WriteLine("truck turns right");
        }
    }

    // 4. The Tester
    class Program
    {
        static void Main(string[] args)
        {
            Car car = new Car();
            car.Drive();

            Truck truck = new Truck();
            truck.Drive();
        }
    }
}