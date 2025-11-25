using System;
using System.Collections.Generic;

namespace BookCatalogue
{
    // [Attributes.h] - Enum Definition
    public enum Genre
    {
        Adventure,
        Classics,
        Detective,
        Fantasy,
        Historic,
        Horror,
        Romance,
        SciFi,
        Unspecified
    }

    // [Attributes.h] - Class Definition
    public class Attributes
    {
        public string Title { get; }
        public string Last { get; }
        public string First { get; }
        public int Year { get; }
        public Genre Gen { get; }

        public Attributes(string title, string last, string first, int year, Genre gen)
        {
            Title = title;
            Last = last;
            First = first;
            Year = year;
            Gen = gen;
        }

        public bool IsMatch(Attributes target)
        {
            // 1. String logic: If target is empty, it's a match.
            //    Otherwise, does the source START with the target? (Mimicking C++ loop logic)
            bool titleMatch = string.IsNullOrEmpty(target.Title) ||
                              Title.StartsWith(target.Title, StringComparison.OrdinalIgnoreCase);

            bool lastMatch = string.IsNullOrEmpty(target.Last) ||
                              Last.StartsWith(target.Last, StringComparison.OrdinalIgnoreCase);

            bool firstMatch = string.IsNullOrEmpty(target.First) ||
                              First.StartsWith(target.First, StringComparison.OrdinalIgnoreCase);

            // 2. Integer logic: 0 acts as wildcard
            bool yearMatch = (target.Year == 0) || (target.Year == Year);

            // 3. Enum logic: Unspecified acts as wildcard
            bool genreMatch = (target.Gen == Genre.Unspecified) || (target.Gen == Gen);

            return titleMatch && lastMatch && firstMatch && yearMatch && genreMatch;
        }

        public override string ToString()
        {
            return $"{{TITLE: '{Title}', LAST: '{Last}', FIRST: '{First}', YEAR: {Year}, GENRE: {Gen.ToString().ToLower()}}}";
        }
    }

    // [Book.h] - Composition Wrapper
    public class Book
    {
        // The Book "owns" the Attributes.
        // In C++, this was a pointer. In C#, it's a reference.
        public Attributes Attrs { get; }

        public Book(Attributes attrs)
        {
            Attrs = attrs;
        }

        // No destructor needed in C# (GC handles memory)
    }

    // [Catalogue.h / .cpp]
    public class Catalogue
    {
        private List<Book> _booklist = new List<Book>();

        public void Add(Attributes attrs)
        {
            _booklist.Add(new Book(attrs));
        }

        public List<Book> Find(Attributes targetAttrs)
        {
            List<Book> matches = new List<Book>();

            foreach (var book in _booklist)
            {
                if (book.Attrs.IsMatch(targetAttrs))
                {
                    matches.Add(book);
                }
            }
            return matches;
        }
    }

    // [tester.cpp]
    class Program
    {
        static void Main(string[] args)
        {
            Catalogue catalogue = new Catalogue();
            Fill(catalogue);
            Test(catalogue);
        }

        static void Fill(Catalogue catalogue)
        {
            catalogue.Add(new Attributes("Life of Pi", "Martel", "Yann", 2003, Genre.Adventure));
            catalogue.Add(new Attributes("The Call of the Wild", "London", "Jack", 1903, Genre.Adventure));
            catalogue.Add(new Attributes("To Kill a Mockingbird", "Lee", "Harper", 1960, Genre.Classics));
            catalogue.Add(new Attributes("Little Women", "Alcott", "Louisa", 1868, Genre.Classics));
            catalogue.Add(new Attributes("The Adventures of Sherlock Holmes", "Doyle", "Conan", 1892, Genre.Detective));
            catalogue.Add(new Attributes("And Then There Were None", "Christie", "Agatha", 1939, Genre.Detective));
            catalogue.Add(new Attributes("Carrie", "King", "Stephen", 1974, Genre.Horror));
            catalogue.Add(new Attributes("It: A Novel", "King", "Stephen", 1986, Genre.Horror));
            catalogue.Add(new Attributes("Frankenstein", "Shelley", "Mary", 1818, Genre.Horror));
            catalogue.Add(new Attributes("2001: A Space Odyssey", "Clarke", "Arthur", 1968, Genre.SciFi));
            catalogue.Add(new Attributes("Ender's Game", "Card", "Orson", 1985, Genre.SciFi));
        }

        static void Search(Catalogue catalogue, Attributes target)
        {
            Console.WriteLine($"\nFind {target}");

            var matches = catalogue.Find(target);

            if (matches.Count == 0)
                Console.WriteLine("No matches.");
            else
            {
                Console.WriteLine("Matches:");
                foreach (var book in matches)
                {
                    Console.WriteLine($"  {book.Attrs}");
                }
            }
        }

        static void Test(Catalogue catalogue)
        {
            Search(catalogue, new Attributes("Life of Pi", "Martel", "Yann", 2003, Genre.Adventure));
            Search(catalogue, new Attributes("", "King", "", 0, Genre.Horror));
            Search(catalogue, new Attributes("1984", "Orwell", "George", 0, Genre.Classics));
            Search(catalogue, new Attributes("", "", "", 1960, Genre.Romance));
            Search(catalogue, new Attributes("", "", "", 1960, Genre.Unspecified));
            Search(catalogue, new Attributes("", "", "", 0, Genre.SciFi));
            Search(catalogue, new Attributes("", "", "", 0, Genre.Unspecified));
        }
    }
}