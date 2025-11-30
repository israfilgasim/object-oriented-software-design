package main

import "fmt"

// Type definitions
type Brake int
type CarWax int
type Direction int
type Engine int
type Headlight int
type Oil int
type Soap int
type Tire int
type Vacuum int

// 1. The Entity: Automobile
type Automobile struct {
	brakes     [4]Brake
	heading    Direction
	headlights [4]Headlight
	speed      int
	tires      [4]Tire
}

func (a *Automobile) Accelerate()    {}
func (a *Automobile) ApplyBrakes()   {}
func (a *Automobile) ShutOffEngine() {}
func (a *Automobile) StartEngine()   {}
func (a *Automobile) TurnLeft()      {}
func (a *Automobile) TurnRight()     {}

// 2. The Service: Garage
type Garage struct {
	car      *Automobile // Explicit pointer required to modify the car
	newOil   Oil
	newTires []Tire // vector<T> becomes a Slice []T
}

// Constructor-like factory function
func NewGarage(c *Automobile) *Garage {
	return &Garage{
		car:      c,
		newTires: make([]Tire, 0), // Initialize empty slice
	}
}

func (g *Garage) AdjustHeadlights() {}
func (g *Garage) ChangeOil()        {}
func (g *Garage) ChangeTires()      {}
func (g *Garage) CheckBrakes()      {}
func (g *Garage) CheckTires()       {}
func (g *Garage) RotateTires()      {}
func (g *Garage) TuneupEngine()     {}

// 3. The Service: CarWash
type CarWash struct {
	car           *Automobile
	soap          Soap
	vacuumCleaner Vacuum
	wax           CarWax
}

func NewCarWash(c *Automobile) *CarWash {
	return &CarWash{car: c}
}

func (w *CarWash) VacuumCar() {}
func (w *CarWash) WashCar()   {}
func (w *CarWash) WaxCar()    {}

// Tester / Main
func main() {
	// 1. Create the car
	myCar := &Automobile{}

	// 2. Wire up the dependencies
	// We pass myCar (which is a pointer) to the services
	myGarage := NewGarage(myCar)
	myWash := NewCarWash(myCar)

	fmt.Printf("Systems Ready: %+v\n", myGarage)
	fmt.Printf("Systems Ready: %+v\n", myWash)
}
