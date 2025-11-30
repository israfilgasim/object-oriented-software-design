package main

import "fmt"

// 1. The Interface
// Defines the required behavior for anything to be considered a "MotorVehicle"
type MotorVehicle interface {
	StartEngine()
	StopEngine()
	Accelerate()
	TurnLeft()
	TurnRight()
	ApplyBrakes()
}

// 2. The "Base" Implementation
// This struct provides the default implementations for methods we might want to reuse.
type DefaultVehicle struct{}

func (d DefaultVehicle) Accelerate() {
	fmt.Println("vehicle accelerates")
}

func (d DefaultVehicle) TurnLeft() {
	fmt.Println("vehicle turns left")
}

func (d DefaultVehicle) TurnRight() {
	fmt.Println("vehicle turns right")
}

func (d DefaultVehicle) ApplyBrakes() {
	fmt.Println("vehicle applies brakes")
}

// Note: DefaultVehicle does NOT implement StartEngine/StopEngine.
// It is incomplete on its own.

// 3. The Car
type Car struct {
	// We embed DefaultVehicle to get Accelerate, Turns, and Brakes for free!
	DefaultVehicle
}

// Car provides the missing pieces
func (c Car) StartEngine() {
	fmt.Println("car starts engine")
}

func (c Car) StopEngine() {
	fmt.Println("car stops engine")
}

// 4. The Truck
type Truck struct {
	DefaultVehicle
}

func (t Truck) StartEngine() {
	fmt.Println("truck starts engine")
}

func (t Truck) StopEngine() {
	fmt.Println("truck stops engine")
}

// Truck "Shadows" (Overrides) the DefaultVehicle's turning methods
func (t Truck) TurnLeft() {
	fmt.Println("truck turns left")
}

func (t Truck) TurnRight() {
	fmt.Println("truck turns right")
}

// 5. The Template Method
// Since Go has no inheritance, we write a function that takes the Interface.
// This allows it to work on BOTH Car and Truck.
func Drive(v MotorVehicle) {
	v.StartEngine()
	v.Accelerate()
	v.TurnLeft()
	v.TurnRight()
	v.ApplyBrakes()
	v.StopEngine()
	fmt.Println()
}

// 6. Main Tester
func main() {
	c := Car{}
	Drive(c) // Works because Car satisfies the MotorVehicle interface

	t := Truck{}
	Drive(t) // Works because Truck satisfies the MotorVehicle interface
}
