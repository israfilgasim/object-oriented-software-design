package main

import (
	"fmt"
	"strings"
)

// [Book.h] - Defined as a struct
type Book struct {
	Title string
	Last  string
	First string
}

// This method satisfies the fmt.Stringer interface
// Equivalent to the C++ operator << overload
func (b Book) String() string {
	return fmt.Sprintf("{TITLE: '%s', LAST: '%s', FIRST: '%s'}", b.Title, b.Last, b.First)
}

// [Catalogue.h] - Defined as a struct holding a slice of pointers to Books
type Catalogue struct {
	booklist []*Book
}

// Go methods use a "receiver" (c *Catalogue) before the function name
func (c *Catalogue) Add(title, last, first string) {
	newBook := &Book{Title: title, Last: last, First: first}
	c.booklist = append(c.booklist, newBook)
}

func (c *Catalogue) Find(target Book) []*Book {
	var matches []*Book

	for _, book := range c.booklist {
		// Logic: If target field is empty, treat as wildcard.
		// Otherwise, use EqualFold for unicode-safe case-insensitive comparison
		if isMatch(target.Title, book.Title) &&
			isMatch(target.Last, book.Last) &&
			isMatch(target.First, book.First) {
			matches = append(matches, book)
		}
	}

	return matches
}

// Helper function
func isMatch(target, source string) bool {
	if target == "" {
		return true
	}
	return strings.EqualFold(target, source)
}

// [tester.cpp] equivalent
func main() {
	catalogue := &Catalogue{}
	fill(catalogue)
	test(catalogue)
}

func fill(c *Catalogue) {
	c.Add("Life of Pi", "Martel", "Yann")
	c.Add("The Call of the Wild", "London", "Jack")
	c.Add("To Kill a Mockingbird", "Lee", "Harper")
	c.Add("Little Women", "Alcott", "Louisa")
	c.Add("The Adventures of Sherlock Holmes", "Doyle", "Conan")
	c.Add("And Then There Were None", "Christie", "Agatha")
	c.Add("Carrie", "King", "Stephen")
	c.Add("It: A Novel", "King", "Stephen")
	c.Add("Frankenstein", "Shelley", "Mary")
	c.Add("2001: A Space Odyssey", "Clarke", "Arthur")
	c.Add("Ender's Game", "Card", "Orson")
}

func search(c *Catalogue, target Book) {
	fmt.Printf("\nFind %s\n", target)

	matches := c.Find(target)

	if len(matches) == 0 {
		fmt.Println("No matches.")
	} else {
		fmt.Println("Matches:")
		for _, book := range matches {
			fmt.Printf("  %s\n", book)
		}
	}
}

func test(c *Catalogue) {
	search(c, Book{Title: "Life of Pi", Last: "Martel", First: "Yann"})
	search(c, Book{Last: "King"}) // Fields default to "" in Go
	search(c, Book{Title: "1984", Last: "Orwell", First: "George"})
	search(c, Book{}) // Empty struct matches everything
}
