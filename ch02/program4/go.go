package main

import (
	"fmt"
	"sort"
	"strings"
)

// ================= 1. ENUMS =================
type Key int

const (
	KEY_KIND Key = iota
	KEY_TITLE
	KEY_LAST
	KEY_FIRST
	KEY_YEAR
	KEY_GENRE
	KEY_REGION
	KEY_SUBJECT
)

var keyNames = [...]string{"KIND", "TITLE", "LAST", "FIRST", "YEAR", "GENRE", "REGION", "SUBJECT"}

func (k Key) String() string { return keyNames[k] }

type Kind int

const (
	FICTION Kind = iota
	COOKBOOK
	HOWTO
)

func (k Kind) String() string { return []string{"fiction", "cookbook", "howto"}[k] }

type Genre int

const (
	ADVENTURE Genre = iota
	CLASSICS
	DETECTIVE
	FANTASY
	HISTORIC
	HORROR
	ROMANCE
	SCIFI
)

func (g Genre) String() string {
	return []string{"adventure", "classics", "detective", "fantasy", "historic", "horror", "romance", "scifi"}[g]
}

type Region int

const (
	CHINA Region = iota
	FRANCE
	INDIA
	ITALY
	MEXICO
	PERSIA
	US
)

func (r Region) String() string {
	return []string{"China", "France", "India", "Italy", "Mexico", "Persia", "US"}[r]
}

type Subject int

const (
	DRAWING Subject = iota
	PAINTING
	WRITING
)

func (s Subject) String() string { return []string{"drawing", "painting", "writing"}[s] }

// ================= 2. ATTRIBUTES =================
type Attributes struct {
	attrMap map[Key]interface{}
}

func NewAttributes(pairs map[Key]interface{}) *Attributes {
	// Validation logic ensuring types match keys
	for k, v := range pairs {
		isValid := false
		switch k {
		case KEY_YEAR:
			_, isValid = v.(int)
		case KEY_TITLE, KEY_LAST, KEY_FIRST:
			_, isValid = v.(string)
		case KEY_KIND:
			_, isValid = v.(Kind)
		case KEY_GENRE:
			_, isValid = v.(Genre)
		case KEY_REGION:
			_, isValid = v.(Region)
		case KEY_SUBJECT:
			_, isValid = v.(Subject)
		}

		if !isValid {
			panic(fmt.Sprintf("Invalid type for Key: %s", k))
		}
	}
	return &Attributes{attrMap: pairs}
}

func (a *Attributes) IsMatch(target *Attributes) bool {
	for tKey, tVal := range target.attrMap {
		sVal, exists := a.attrMap[tKey]
		if !exists {
			return false
		}
		// Exact Match
		if sVal == tVal {
			continue
		}
		// String Case-Insensitive Match
		if tStr, ok1 := tVal.(string); ok1 {
			if sStr, ok2 := sVal.(string); ok2 {
				if strings.EqualFold(sStr, tStr) {
					continue
				}
			}
		}
		return false
	}
	return true
}

func (a *Attributes) String() string {
	// Sort keys for deterministic output
	var keys []int
	for k := range a.attrMap {
		keys = append(keys, int(k))
	}
	sort.Ints(keys)

	var sb strings.Builder
	sb.WriteString("{")
	for i, kInt := range keys {
		if i > 0 {
			sb.WriteString(", ")
		}
		k := Key(kInt)
		sb.WriteString(k.String() + ": ")

		v := a.attrMap[k]
		switch val := v.(type) {
		case string:
			sb.WriteString(fmt.Sprintf("'%s'", val))
		case fmt.Stringer:
			sb.WriteString(val.String())
		default:
			sb.WriteString(fmt.Sprintf("%v", val))
		}
	}
	sb.WriteString("}")
	return sb.String()
}

// ================= 3. BOOK & CATALOGUE =================
type Book struct {
	Attrs *Attributes
}

func (b Book) String() string { return b.Attrs.String() }

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

// ================= 4. TESTER (MAIN) =================

// Helper type alias to make the filling code cleaner
type M map[Key]interface{}

func main() {
	catalogue := &Catalogue{}
	fill(catalogue)
	test(catalogue)
}

func fill(c *Catalogue) {
	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "Life of Pi",
		KEY_LAST: "Martel", KEY_FIRST: "Yann",
		KEY_YEAR: 2003, KEY_GENRE: ADVENTURE,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "The Call of the Wild",
		KEY_LAST: "London", KEY_FIRST: "Jack",
		KEY_YEAR: 1903, KEY_GENRE: ADVENTURE,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "To Kill a Mockingbird",
		KEY_LAST: "Lee", KEY_FIRST: "Harper",
		KEY_YEAR: 1960, KEY_GENRE: CLASSICS,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "Little Women",
		KEY_LAST: "Alcott", KEY_FIRST: "Louisa",
		KEY_YEAR: 1868, KEY_GENRE: CLASSICS,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "The Adventures of Sherlock Holmes",
		KEY_LAST: "Doyle", KEY_FIRST: "Conan",
		KEY_YEAR: 1892, KEY_GENRE: DETECTIVE,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "And Then There Were None",
		KEY_LAST: "Christie", KEY_FIRST: "Agatha",
		KEY_YEAR: 1939, KEY_GENRE: DETECTIVE,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "Carrie",
		KEY_LAST: "King", KEY_FIRST: "Stephen",
		KEY_YEAR: 1974, KEY_GENRE: HORROR,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "It: A Novel",
		KEY_LAST: "King", KEY_FIRST: "Stephen",
		KEY_YEAR: 1986, KEY_GENRE: HORROR,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "Frankenstein",
		KEY_LAST: "Shelley", KEY_FIRST: "Mary",
		KEY_YEAR: 1818, KEY_GENRE: HORROR,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "2001: A Space Odyssey",
		KEY_LAST: "Clarke", KEY_FIRST: "Arthur",
		KEY_YEAR: 1968, KEY_GENRE: SCIFI,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "Ender's Game",
		KEY_LAST: "Card", KEY_FIRST: "Orson",
		KEY_YEAR: 1985, KEY_GENRE: SCIFI,
	}))

	// Cookbooks
	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "The Wok of Life",
		KEY_LAST: "Leung", KEY_FIRST: "Bill", KEY_REGION: CHINA,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Chinese Cooking for Dummies",
		KEY_LAST: "Yan", KEY_FIRST: "Martin", KEY_REGION: CHINA,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Mastering the Art of French Cooking",
		KEY_LAST: "Child", KEY_FIRST: "Julia", KEY_REGION: FRANCE,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Vegetarian India",
		KEY_LAST: "Jaffrey", KEY_FIRST: "Madhur", KEY_REGION: INDIA,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Made in India",
		KEY_LAST: "Sodha", KEY_FIRST: "Meera", KEY_REGION: INDIA,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Essentials of Classic Italian Cooking",
		KEY_LAST: "Hazan", KEY_FIRST: "Marcella", KEY_REGION: ITALY,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "The Complete Italian Cookbook",
		KEY_LAST: "Mazzocco", KEY_FIRST: "Manuela", KEY_REGION: ITALY,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Food for Life",
		KEY_LAST: "Batmanglij", KEY_FIRST: "Najmieh", KEY_REGION: PERSIA,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "The New Orleans Kitchen",
		KEY_LAST: "Devillier", KEY_FIRST: "Justin", KEY_REGION: US,
	}))

	c.Add(NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Rodney Scott's World of BBQ",
		KEY_LAST: "Scott", KEY_FIRST: "Rodney", KEY_REGION: US,
	}))

	// HowTo
	c.Add(NewAttributes(M{
		KEY_KIND: HOWTO, KEY_TITLE: "On Writing: A Memoir of the Craft",
		KEY_LAST: "King", KEY_FIRST: "Stephen", KEY_SUBJECT: WRITING,
	}))
}

func search(c *Catalogue, target *Attributes) {
	fmt.Printf("\nFind %s\n", target)
	matches := c.Find(target)
	if len(matches) == 0 {
		fmt.Println("No matches.")
	} else {
		fmt.Println("Matches:")
		for _, b := range matches {
			fmt.Printf("  %s\n", b)
		}
	}
}

func test(c *Catalogue) {
	search(c, NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "Life of Pi",
		KEY_LAST: "Martel", KEY_FIRST: "Yann",
		KEY_YEAR: 2003, KEY_GENRE: ADVENTURE,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: FICTION, KEY_LAST: "KING", KEY_GENRE: HORROR,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: FICTION, KEY_TITLE: "1984",
		KEY_LAST: "Orwell", KEY_FIRST: "George", KEY_GENRE: CLASSICS,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: FICTION, KEY_YEAR: 1960, KEY_GENRE: ROMANCE,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: FICTION, KEY_YEAR: 1960,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: FICTION, KEY_GENRE: SCIFI,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: FICTION,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_TITLE: "Mastering the Art of French Cooking",
		KEY_LAST: "Child", KEY_FIRST: "Julia", KEY_REGION: FRANCE,
	}))

	search(c, NewAttributes(M{
		KEY_REGION: CHINA,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_LAST: "Leung", KEY_REGION: MEXICO,
	}))

	search(c, NewAttributes(M{
		KEY_KIND: COOKBOOK, KEY_LAST: "Scott", KEY_FIRST: "Rodney",
	}))

	search(c, NewAttributes(M{
		KEY_LAST: "King",
	}))
}
