package main

import (
	"fmt"
	"strings"
)

// --- Enums ---
type Genre int

const (
	Adventure Genre = iota
	Classics
	Detective
	Horror
	Romance
	SciFi
	UnspecifiedGenre
)

// String() method for Genre omitted for brevity, similar to previous example

type Region int

const (
	China Region = iota
	France
	India
	Italy
	Mexico
	US
	UnspecifiedRegion
)

// String() method for Region omitted for brevity

// --- Attributes (Composition) ---

// Base struct
type Attributes struct {
	Title, Last, First string
}

func (a *Attributes) IsMatchBase(target *Attributes) bool {
	// Helper to reduce code duplication
	check := func(tgt, src string) bool {
		return tgt == "" || (len(src) >= len(tgt) && strings.EqualFold(src[:len(tgt)], tgt))
	}
	return check(target.Title, a.Title) &&
		check(target.Last, a.Last) &&
		check(target.First, a.First)
}

// Fiction Attributes
type FictionAttrs struct {
	Attributes // Embedding Attributes (anonymous field)
	Year       int
	Gen        Genre
}

func (f *FictionAttrs) IsMatch(target *FictionAttrs) bool {
	if !f.IsMatchBase(&target.Attributes) {
		return false
	}
	return (target.Year == 0 || target.Year == f.Year) &&
		(target.Gen == UnspecifiedGenre || target.Gen == f.Gen)
}

func (f *FictionAttrs) String() string {
	// Note: We access embedded fields directly (f.Title)
	return fmt.Sprintf("{TITLE: '%s', LAST: '%s', FIRST: '%s', YEAR: %d, GENRE: %d}",
		f.Title, f.Last, f.First, f.Year, f.Gen)
}

// Cookbook Attributes
type CookbookAttrs struct {
	Attributes
	Reg Region
}

func (c *CookbookAttrs) IsMatch(target *CookbookAttrs) bool {
	if !c.IsMatchBase(&target.Attributes) {
		return false
	}
	return (target.Reg == UnspecifiedRegion || target.Reg == c.Reg)
}

func (c *CookbookAttrs) String() string {
	return fmt.Sprintf("{TITLE: '%s', LAST: '%s', FIRST: '%s', REGION: %d}",
		c.Title, c.Last, c.First, c.Reg)
}

// --- Book Hierarchy ---

// We define an interface for anything that can be a "Book"
// (Even if it's just a marker interface for now)
type Book interface {
	GetAttributes() interface{}
}

// Concrete Fiction Book
type Fiction struct {
	Attrs *FictionAttrs
}

func (f *Fiction) GetAttributes() interface{} { return f.Attrs }

// Concrete Cookbook Book
type Cookbook struct {
	Attrs *CookbookAttrs
}

func (c *Cookbook) GetAttributes() interface{} { return c.Attrs }

// --- Catalogue ---

type Catalogue struct {
	// We store a slice of the Book interface
	booklist []Book
}

func (c *Catalogue) AddFiction(attrs *FictionAttrs) {
	c.booklist = append(c.booklist, &Fiction{Attrs: attrs})
}

func (c *Catalogue) AddCookbook(attrs *CookbookAttrs) {
	c.booklist = append(c.booklist, &Cookbook{Attrs: attrs})
}

// FindFiction returns only *Fiction types
func (c *Catalogue) FindFiction(target *FictionAttrs) []*Fiction {
	var matches []*Fiction

	for _, book := range c.booklist {
		// TYPE ASSERTION (Go's dynamic_cast)
		// We ask: "Is this generic 'book' variable actually a pointer to Fiction?"
		if fictionBook, ok := book.(*Fiction); ok {
			// If ok is true, fictionBook is now type *Fiction
			if fictionBook.Attrs.IsMatch(target) {
				matches = append(matches, fictionBook)
			}
		}
	}
	return matches
}

func (c *Catalogue) FindCookbook(target *CookbookAttrs) []*Cookbook {
	var matches []*Cookbook

	for _, book := range c.booklist {
		// TYPE ASSERTION
		if cookbook, ok := book.(*Cookbook); ok {
			if cookbook.Attrs.IsMatch(target) {
				matches = append(matches, cookbook)
			}
		}
	}
	return matches
}

// --- Tester ---

func main() {
	cat := &Catalogue{}
	fill(cat)
	test(cat)
}

func fill(c *Catalogue) {
	c.AddFiction(&FictionAttrs{Attributes{"Life of Pi", "Martel", "Yann"}, 2003, Adventure})
	c.AddFiction(&FictionAttrs{Attributes{"The Call of the Wild", "London", "Jack"}, 1903, Adventure})
	c.AddCookbook(&CookbookAttrs{Attributes{"The Woks of Life", "Leung", "Bill"}, China})
	c.AddCookbook(&CookbookAttrs{Attributes{"Mastering the Art of French Cooking", "Child", "Julia"}, France})
}

func searchFiction(c *Catalogue, target *FictionAttrs) {
	fmt.Printf("\nFind %s\n", target)
	matches := c.FindFiction(target)
	if len(matches) == 0 {
		fmt.Println("No matches.")
	} else {
		fmt.Println("Matches:")
		for _, f := range matches {
			fmt.Printf("  %s\n", f.Attrs)
		}
	}
}

func searchCookbook(c *Catalogue, target *CookbookAttrs) {
	fmt.Printf("\nFind %s\n", target)
	matches := c.FindCookbook(target)
	if len(matches) == 0 {
		fmt.Println("No matches.")
	} else {
		fmt.Println("Matches:")
		for _, cb := range matches {
			fmt.Printf("  %s\n", cb.Attrs)
		}
	}
}

func test(c *Catalogue) {
	searchFiction(c, &FictionAttrs{Attributes{"Life", "Martel", "Yann"}, 2003, Adventure})
	searchFiction(c, &FictionAttrs{Attributes{"", "King", ""}, 0, Horror})
	searchCookbook(c, &CookbookAttrs{Attributes{"Mastering the Art of French Cooking", "Child", "Julia"}, France})
}
