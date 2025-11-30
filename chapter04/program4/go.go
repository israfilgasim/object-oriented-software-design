package main

import "fmt"

// Type Definitions
type Brake int
type Direction int
type Engine int

// 1. The Interface
// Defines the behavior required to be a "MotorVehicle"
type MotorVehicle interface {
	StartEngine()
	StopEngine()
	Accelerate()
	TurnLeft()
	TurnRight()
	ApplyBrakes()
	Drive()
}

// 2. The Car Implementation
type Car struct {
	brakes  [4]Brake
	engine  Engine
	heading Direction
	speed   int
}

// Implementing the methods automatically satisfies the interface
func (c *Car) StartEngine() { fmt.Println("car starts engine") }
func (c *Car) StopEngine()  { fmt.Println("car stops engine") }
func (c *Car) Accelerate()  { fmt.Println("car accelerates") }
func (c *Car) TurnLeft()    { fmt.Println("car turns left") }
func (c *Car) TurnRight()   { fmt.Println("car turns right") }
func (c *Car) ApplyBrakes() { fmt.Println("car applies brakes") }

func (c *Car) Drive() {
	c.StartEngine()
	c.Accelerate()
	c.TurnLeft()
	c.TurnRight()
	c.ApplyBrakes()
	c.StopEngine()
	fmt.Println()
}

// 3. The Truck Implementation
type Truck struct {
	brakes  [4]Brake
	engine  Engine
	heading Direction
	speed   int
}

func (t *Truck) StartEngine() { fmt.Println("truck starts engine") }
func (t *Truck) StopEngine()  { fmt.Println("truck stops engine") }
func (t *Truck) Accelerate()  { fmt.Println("truck accelerates") }
func (t *Truck) TurnLeft()    { fmt.Println("truck turns left") }
func (t *Truck) TurnRight()   { fmt.Println("truck turns right") }
func (t *Truck) ApplyBrakes() { fmt.Println("truck applies brakes") }

func (t *Truck) Drive() {
	t.StartEngine()
	t.Accelerate()
	t.TurnLeft()
	t.TurnRight()
	t.ApplyBrakes()
	t.StopEngine()
	fmt.Println()
}

// 4. Tester
func main() {
	// Create concrete types
	car := &Car{}
	truck := &Truck{}

	// Because they implement the methods, they ARE MotorVehicles.
	// We can put them in a list of interfaces to prove it:
	vehicles := []MotorVehicle{car, truck}

	for _, v := range vehicles {
		v.Drive()
	}
}
