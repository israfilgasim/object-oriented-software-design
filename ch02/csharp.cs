using System;
using System.Collections.Generic;

namespace BookCatalogue
{
    // [Book.h] equivalent
    public class Book
    {
        // Properties with private setters (immutable-ish, like the C++ const getters)
        public string Title { get; private set; }
        public string Last { get; private set; }
        public string First { get; private set; }

        public Book(string title, string last, string first)
        {
            Title = title;
            Last = last;
            First = first;
        }

        // Replacing the << operator overload with the standard ToString override
        public override string ToString()
        {
            return $"{{TITLE: '{Title}', LAST: '{Last}', FIRST: '{First}'}}";
        }
    }

    // [Catalogue.h and Catalogue.cpp] combined
    public class Catalogue
    {
        private List<Book> _booklist = new List<Book>();

        public void Add(string title, string last, string first)
        {
            // C# manages memory automatically. No 'delete' needed later.
            _booklist.Add(new Book(title, last, first));
        }

        public List<Book> Find(Book target)
        {
            List<Book> matches = new List<Book>();

            foreach (var book in _booklist)
            {
                // Logic: If target field is empty, treat as wildcard (match anything)
                // Otherwise, check for case-insensitive match.
                if (IsMatch(target.Title, book.Title) &&
                    IsMatch(target.Last, book.Last) &&
                    IsMatch(target.First, book.First))
                {
                    matches.Add(book);
                }
            }

            return matches;
        }

        // Helper to replicate the C++ logic where empty string returns true
        private static bool IsMatch(string target, string source)
        {
            if (string.IsNullOrEmpty(target)) return true;
            return string.Equals(target, source, StringComparison.OrdinalIgnoreCase);
        }
    }

    // [tester.cpp] equivalent
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
            catalogue.Add("Life of Pi", "Martel", "Yann");
            catalogue.Add("The Call of the Wild", "London", "Jack");
            catalogue.Add("To Kill a Mockingbird", "Lee", "Harper");
            catalogue.Add("Little Women", "Alcott", "Louisa");
            catalogue.Add("The Adventures of Sherlock Holmes", "Doyle", "Conan");
            catalogue.Add("And Then There Were None", "Christie", "Agatha");
            catalogue.Add("Carrie", "King", "Stephen");
            catalogue.Add("It: A Novel", "King", "Stephen");
            catalogue.Add("Frankenstein", "Shelley", "Mary");
            catalogue.Add("2001: A Space Odyssey", "Clarke", "Arthur");
            catalogue.Add("Ender's Game", "Card", "Orson");
        }

        static void Search(Catalogue catalogue, Book target)
        {
            Console.WriteLine($"\nFind {target}");

            var matches = catalogue.Find(target);

            if (matches.Count == 0)
            {
                Console.WriteLine("No matches.");
            }
            else
            {
                Console.WriteLine("Matches:");
                foreach (var book in matches)
                {
                    Console.WriteLine($"  {book}");
                }
            }
        }

        static void Test(Catalogue catalogue)
        {
            Search(catalogue, new Book("Life of Pi", "Martel", "Yann"));
            Search(catalogue, new Book("", "King", "")); // Search by Last Name only
            Search(catalogue, new Book("1984", "Orwell", "George"));
            Search(catalogue, new Book("", "", "")); // Dump all books
        }
    }
}
