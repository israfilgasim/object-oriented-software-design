using System;
using System.Collections.Generic;

namespace LibrarySystem
{
    // ================= ENUMS =================
    public enum Genre
    {
        Adventure, Classics, Detective, Fantasy, Historic,
        Horror, Romance, SciFi, Unspecified
    }

    public enum Region
    {
        China, France, India, Italy, Mexico, Persia, US, Unspecified
    }

    // ================= ATTRIBUTES (HIERARCHY) =================
    // [Attributes.h]
    public class Attributes
    {
        public string Title { get; }
        public string Last { get; }
        public string First { get; }

        public Attributes(string title, string last, string first)
        {
            Title = title;
            Last = last;
            First = first;
        }

        // Virtual method allows children to extend matching logic
        public virtual bool IsMatch(Attributes target)
        {
            return (string.IsNullOrEmpty(target.Title) || Title.StartsWith(target.Title, StringComparison.OrdinalIgnoreCase)) &&
                   (string.IsNullOrEmpty(target.Last) || Last.StartsWith(target.Last, StringComparison.OrdinalIgnoreCase)) &&
                   (string.IsNullOrEmpty(target.First) || First.StartsWith(target.First, StringComparison.OrdinalIgnoreCase));
        }
    }

    // [FictionAttrs.h]
    public class FictionAttrs : Attributes
    {
        public int Year { get; }
        public Genre Gen { get; }

        public FictionAttrs(string title, string last, string first, int year, Genre gen)
            : base(title, last, first)
        {
            Year = year;
            Gen = gen;
        }

        public override bool IsMatch(Attributes targetBase)
        {
            // First check base match
            if (!base.IsMatch(targetBase)) return false;

            // Safe cast: ensure target is also FictionAttrs
            if (targetBase is FictionAttrs target)
            {
                bool yearMatch = (target.Year == 0) || (target.Year == Year);
                bool genreMatch = (target.Gen == Genre.Unspecified) || (target.Gen == Gen);
                return yearMatch && genreMatch;
            }
            return false;
        }

        public override string ToString() =>
            $"{{TITLE: '{Title}', LAST: '{Last}', FIRST: '{First}', YEAR: {Year}, GENRE: {Gen}}}";
    }

    // [CookbookAttrs.h]
    public class CookbookAttrs : Attributes
    {
        public Region Reg { get; }

        public CookbookAttrs(string title, string last, string first, Region reg)
            : base(title, last, first)
        {
            Reg = reg;
        }

        public override bool IsMatch(Attributes targetBase)
        {
            if (!base.IsMatch(targetBase)) return false;

            if (targetBase is CookbookAttrs target)
            {
                return (target.Reg == Region.Unspecified) || (target.Reg == Reg);
            }
            return false;
        }

        public override string ToString() =>
            $"{{TITLE: '{Title}', LAST: '{Last}', FIRST: '{First}', REGION: {Reg}}}";
    }

    // ================= BOOK (HIERARCHY) =================
    // [Book.h]
    public class Book
    {
        public Attributes Attrs { get; }
        public Book(Attributes attrs) { Attrs = attrs; }
    }

    // [Fiction.h] - Inherits Book
    public class Fiction : Book
    {
        public Fiction(FictionAttrs attrs) : base(attrs) { }
    }

    // [Cookbook.h] - Inherits Book
    public class Cookbook : Book
    {
        public Cookbook(CookbookAttrs attrs) : base(attrs) { }
    }

    // ================= CATALOGUE =================
    // [Catalogue.h / .cpp]
    public class Catalogue
    {
        // Polymorphic list: stores Fiction and Cookbooks mixed together
        private List<Book> _booklist = new List<Book>();

        public void Add(FictionAttrs attrs) => _booklist.Add(new Fiction(attrs));
        public void Add(CookbookAttrs attrs) => _booklist.Add(new Cookbook(attrs));

        // Returns only Fiction books that match
        public List<Fiction> Find(FictionAttrs target)
        {
            List<Fiction> matches = new List<Fiction>();
            foreach (var book in _booklist)
            {
                // C# Equivalent of dynamic_cast: 'is'
                // Checks if 'book' is of type 'Fiction' and assigns it to 'f'
                if (book is Fiction f)
                {
                    // Logic: We know f.Attrs is FictionAttrs because the constructor forced it.
                    if (f.Attrs.IsMatch(target))
                    {
                        matches.Add(f);
                    }
                }
            }
            return matches;
        }

        public List<Cookbook> Find(CookbookAttrs target)
        {
            List<Cookbook> matches = new List<Cookbook>();
            foreach (var book in _booklist)
            {
                if (book is Cookbook cb)
                {
                    if (cb.Attrs.IsMatch(target))
                    {
                        matches.Add(cb);
                    }
                }
            }
            return matches;
        }
    }

    // ================= TESTER =================
    class Program
    {
        static void Main(string[] args)
        {
            Catalogue catalogue = new Catalogue();
            Fill(catalogue);
            Test(catalogue);
        }

        static void Fill(Catalogue c)
        {
            c.Add(new FictionAttrs("Life of Pi", "Martel", "Yann", 2003, Genre.Adventure));
            c.Add(new FictionAttrs("The Call of the Wild", "London", "Jack", 1903, Genre.Adventure));
            c.Add(new FictionAttrs("To Kill a Mockingbird", "Lee", "Harper", 1960, Genre.Classics));
            c.Add(new FictionAttrs("Little Women", "Alcott", "Louisa", 1868, Genre.Classics));
            c.Add(new CookbookAttrs("The Woks of Life", "Leung", "Bill", Region.China));
            c.Add(new CookbookAttrs("Mastering the Art of French Cooking", "Child", "Julia", Region.France));
            // ... (Other adds omitted for brevity, logic matches C++)
        }

        static void SearchFiction(Catalogue c, FictionAttrs target)
        {
            Console.WriteLine($"\nFind {target}");
            var matches = c.Find(target);
            if (matches.Count == 0) Console.WriteLine("No matches.");
            else
            {
                Console.WriteLine("Matches:");
                foreach (var f in matches) Console.WriteLine($"  {f.Attrs}");
            }
        }

        static void SearchCookbook(Catalogue c, CookbookAttrs target)
        {
            Console.WriteLine($"\nFind {target}");
            var matches = c.Find(target);
            if (matches.Count == 0) Console.WriteLine("No matches.");
            else
            {
                Console.WriteLine("Matches:");
                foreach (var cb in matches) Console.WriteLine($"  {cb.Attrs}");
            }
        }

        static void Test(Catalogue c)
        {
            SearchFiction(c, new FictionAttrs("Life of Pi", "Martel", "Yann", 2003, Genre.Adventure));
            SearchFiction(c, new FictionAttrs("", "King", "", 0, Genre.Horror)); // Wildcards
            SearchCookbook(c, new CookbookAttrs("Mastering the Art of French Cooking", "Child", "Julia", Region.France));
            SearchCookbook(c, new CookbookAttrs("", "", "", Region.China));
        }
    }
}