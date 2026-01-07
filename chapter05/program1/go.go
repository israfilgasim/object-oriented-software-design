package main

import (
	"fmt"
	"log"
)

// Item struct definition
type Item struct {
	// fields are lowercase (unexported/private) to enforce encapsulation
	name   string
	weight float64
	price  float64
}

// NewItem is the "Constructor" pattern (Factory function)
func NewItem(name string, weight, price float64) *Item {
	return &Item{
		name:   name,
		weight: weight,
		price:  price,
	}
}

// Name getter (Note: idiomatic Go drops the "Get" prefix)
func (i *Item) Name() string {
	return i.name
}

// Weight getter
func (i *Item) Weight() float64 {
	return i.weight
}

// Price getter
func (i *Item) Price() float64 {
	return i.price
}

// SetPrice setter
// Idiomatic Go: Return an error if validation fails.
func (i *Item) SetPrice(p float64) error {
	if p <= 0 {
		return fmt.Errorf("invalid price: %.2f, must be > 0", p)
	}
	i.price = p
	return nil
}

func main() {
	item := NewItem("whole chicken", 4.5, 10.31)

	fmt.Println("  name:", item.Name())
	fmt.Println("weight:", item.Weight(), "lbs")
	fmt.Printf(" price: $%.2f\n", item.Price())

	// Attempting to set an invalid price
	// Idiomatic Go: Check the error returned by the function
	err := item.SetPrice(-9.99)
	if err != nil {
		log.Printf("Failed to set price: %v\n", err)
	} else {
		fmt.Printf("\nnew price: $%.2f\n", item.Price())
	}
}
