package main

import (
	"fmt"
	"strings"
)

// [Attributes.h] - Defining Enum using 'iota'
type Genre int

const (
	Adventure Genre = iota
	Classics
	Detective
	Fantasy
	Historic
	Horror
	Romance
	SciFi
	Unspecified
)

// Manual string mapping for the Enum
func (g Genre) String() string {
	names := []string{
		"adventure", "classics", "detective", "fantasy",
		"historic", "horror", "romance", "scifi", "unspecified",
	}
	if int(g) < len(names) {
		return names[g]
	}
	return "unspecified"
}

// [Attributes.h] - Struct
type Attributes struct {
	Title string
	Last  string
	First string
	Year  int
	Gen   Genre
}

func (a *Attributes) IsMatch(target *Attributes) bool {
	// String Logic: Check "Starts With" case-insensitive
	titleMatch := matchesStr(target.Title, a.Title)
	lastMatch := matchesStr(target.Last, a.Last)
	firstMatch := matchesStr(target.First, a.First)

	// Int Logic: 0 is wildcard
	yearMatch := (target.Year == 0) || (target.Year == a.Year)

	// Enum Logic: Unspecified is wildcard
	genreMatch := (target.Gen == Unspecified) || (target.Gen == a.Gen)

	return titleMatch && lastMatch && firstMatch && yearMatch && genreMatch
}

// Helper to mimic the C++ partial match loop
func matchesStr(target, source string) bool {
	if target == "" {
		return true
	}
	// Check if source starts with target (Case Insensitive)
	if len(target) > len(source) {
		return false
	}
	return strings.EqualFold(source[:len(target)], target)
}

func (a *Attributes) String() string {
	return fmt.Sprintf("{TITLE: '%s', LAST: '%s', FIRST: '%s', YEAR: %d, GENRE: %s}",
		a.Title, a.Last, a.First, a.Year, a.Gen)
}

// [Book.h]
type Book struct {
	// Composition: Book holds a pointer to Attributes
	Attrs *Attributes
}

// [Catalogue.h]
type Catalogue struct {
	booklist []*Book
}

func (c *Catalogue) Add(attrs *Attributes) {
	c.booklist = append(c.booklist, &Book{Attrs: attrs})
}

func (c *Catalogue) Find(target *Attributes) []*Book {
	var matches []*Book
	for _, book := range c.booklist {
		if book.Attrs.IsMatch(target) {
			matches = append(matches, book)
		}
	}
	return matches
}

// [tester.cpp]
func main() {
	catalogue := &Catalogue{}
	fill(catalogue)
	test(catalogue)
}

func fill(c *Catalogue) {
	c.Add(&Attributes{"Life of Pi", "Martel", "Yann", 2003, Adventure})
	c.Add(&Attributes{"The Call of the Wild", "London", "Jack", 1903, Adventure})
	c.Add(&Attributes{"To Kill a Mockingbird", "Lee", "Harper", 1960, Classics})
	c.Add(&Attributes{"Little Women", "Alcott", "Louisa", 1868, Classics})
	c.Add(&Attributes{"The Adventures of Sherlock Holmes", "Doyle", "Conan", 1892, Detective})
	c.Add(&Attributes{"And Then There Were None", "Christie", "Agatha", 1939, Detective})
	c.Add(&Attributes{"Carrie", "King", "Stephen", 1974, Horror})
	c.Add(&Attributes{"It: A Novel", "King", "Stephen", 1986, Horror})
	c.Add(&Attributes{"Frankenstein", "Shelley", "Mary", 1818, Horror})
	c.Add(&Attributes{"2001: A Space Odyssey", "Clarke", "Arthur", 1968, SciFi})
	c.Add(&Attributes{"Ender's Game", "Card", "Orson", 1985, SciFi})
}

func search(c *Catalogue, target *Attributes) {
	fmt.Printf("\nFind %s\n", target)
	matches := c.Find(target)

	if len(matches) == 0 {
		fmt.Println("No matches.")
	} else {
		fmt.Println("Matches:")
		for _, book := range matches {
			fmt.Printf("  %s\n", book.Attrs)
		}
	}
}

func test(c *Catalogue) {
	search(c, &Attributes{Title: "Life of Pi", Last: "Martel", First: "Yann", Year: 2003, Gen: Adventure})
	// Note: In Go, 0 is the default value for int, so we don't need to type it explicitly if we want 0.
	search(c, &Attributes{Last: "King", Gen: Horror})
	search(c, &Attributes{Title: "1984", Last: "Orwell", First: "George", Gen: Classics})
	search(c, &Attributes{Year: 1960, Gen: Romance})
	search(c, &Attributes{Year: 1960, Gen: Unspecified})
	search(c, &Attributes{Gen: SciFi})
	search(c, &Attributes{Gen: Unspecified})
}
