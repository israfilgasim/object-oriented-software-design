using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem
{
    // ================= 1. ENUMS =================
    public enum Key
    {
        Kind, Title, Last, First, Year, Genre, Region, Subject
    }

    public enum Kind { Fiction, Cookbook, HowTo }
    public enum Genre { Adventure, Classics, Detective, Fantasy, Historic, Horror, Romance, SciFi }
    public enum Region { China, France, India, Italy, Mexico, Persia, US }
    public enum Subject { Drawing, Painting, Writing }

    // ================= 2. ATTRIBUTES =================
    public class Attributes
    {
        // Replaces std::map<Key, variant>
        private Dictionary<Key, object> _map;

        public Attributes(Dictionary<Key, object> pairs)
        {
            Validate(pairs);
            _map = pairs;
        }

        // Ensures type safety (mimicking C++ assertions)
        private void Validate(Dictionary<Key, object> pairs)
        {
            foreach (var kvp in pairs)
            {
                Key key = kvp.Key;
                object val = kvp.Value;
                bool isValid = key switch
                {
                    Key.Year => val is int,
                    Key.Title or Key.Last or Key.First => val is string,
                    Key.Kind => val is Kind,
                    Key.Genre => val is Genre,
                    Key.Region => val is Region,
                    Key.Subject => val is Subject,
                    _ => false
                };

                if (!isValid)
                    throw new ArgumentException($"Invalid type for Key: {key}");
            }
        }

        public bool IsMatch(Attributes targetAttrs)
        {
            // Logic: Iterate over TARGET keys. 
            // If the target specifies a key, the source MUST match it.
            // If the target omits a key, it acts as a wildcard.
            foreach (var kvp in targetAttrs._map)
            {
                Key targetKey = kvp.Key;
                object targetValue = kvp.Value;

                if (!_map.ContainsKey(targetKey)) return false;

                object sourceValue = _map[targetKey];

                // Check 1: Exact Match (Enums, Ints)
                if (sourceValue.Equals(targetValue)) continue;

                // Check 2: String Case-Insensitive Match
                if (targetValue is string targetStr && sourceValue is string sourceStr)
                {
                    if (string.Equals(sourceStr, targetStr, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                return false;
            }
            return true;
        }

        public override string ToString()
        {
            // Sorting keys to ensure deterministic output like C++
            var sortedKeys = _map.Keys.OrderBy(k => k);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            bool first = true;

            foreach (var key in sortedKeys)
            {
                if (!first) sb.Append(", ");
                first = false;

                sb.Append($"{key.ToString().ToUpper()}: ");

                object val = _map[key];
                if (val is string)
                    sb.Append($"'{val}'");
                else if (val is Enum)
                    sb.Append(val.ToString().ToLower());
                else
                    sb.Append(val);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    // ================= 3. BOOK & CATALOGUE =================
    public class Book
    {
        public Attributes Attrs { get; }
        public Book(Attributes attrs) { Attrs = attrs; }
        public override string ToString() => Attrs.ToString();
    }

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

    // ================= 4. TESTER (MAIN) =================
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
            // C# Dictionary Initializer Syntax
            catalogue.Add(new Attributes(new Dictionary<Key, object>
            {
                {Key.Kind,  Kind.Fiction},
                {Key.Title, "Life of Pi"},
                {Key.Last,  "Martel"},
                {Key.First, "Yann"},
                {Key.Year,  2003},
                {Key.Genre, Genre.Adventure}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object>
            {
                {Key.Kind,  Kind.Fiction},
                {Key.Title, "The Call of the Wild"},
                {Key.Last,  "London"},
                {Key.First, "Jack"},
                {Key.Year,  1903},
                {Key.Genre, Genre.Adventure}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind,  Kind.Fiction}, {Key.Title, "To Kill a Mockingbird"},
                {Key.Last,  "Lee"}, {Key.First, "Harper"},
                {Key.Year,  1960}, {Key.Genre, Genre.Classics}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "Little Women"},
                {Key.Last, "Alcott"}, {Key.First, "Louisa"},
                {Key.Year, 1868}, {Key.Genre, Genre.Classics}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "The Adventures of Sherlock Holmes"},
                {Key.Last, "Doyle"}, {Key.First, "Conan"},
                {Key.Year, 1892}, {Key.Genre, Genre.Detective}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "And Then There Were None"},
                {Key.Last, "Christie"}, {Key.First, "Agatha"},
                {Key.Year, 1939}, {Key.Genre, Genre.Detective}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "Carrie"},
                {Key.Last, "King"}, {Key.First, "Stephen"},
                {Key.Year, 1974}, {Key.Genre, Genre.Horror}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "It: A Novel"},
                {Key.Last, "King"}, {Key.First, "Stephen"},
                {Key.Year, 1986}, {Key.Genre, Genre.Horror}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "Frankenstein"},
                {Key.Last, "Shelley"}, {Key.First, "Mary"},
                {Key.Year, 1818}, {Key.Genre, Genre.Horror}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "2001: A Space Odyssey"},
                {Key.Last, "Clarke"}, {Key.First, "Arthur"},
                {Key.Year, 1968}, {Key.Genre, Genre.SciFi}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "Ender's Game"},
                {Key.Last, "Card"}, {Key.First, "Orson"},
                {Key.Year, 1985}, {Key.Genre, Genre.SciFi}
            }));

            // Cookbooks
            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "The Wok of Life"},
                {Key.Last, "Leung"}, {Key.First, "Bill"}, {Key.Region, Region.China}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Chinese Cooking for Dummies"},
                {Key.Last, "Yan"}, {Key.First, "Martin"}, {Key.Region, Region.China}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Mastering the Art of French Cooking"},
                {Key.Last, "Child"}, {Key.First, "Julia"}, {Key.Region, Region.France}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Vegetarian India"},
                {Key.Last, "Jaffrey"}, {Key.First, "Madhur"}, {Key.Region, Region.India}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Made in India"},
                {Key.Last, "Sodha"}, {Key.First, "Meera"}, {Key.Region, Region.India}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Essentials of Classic Italian Cooking"},
                {Key.Last, "Hazan"}, {Key.First, "Marcella"}, {Key.Region, Region.Italy}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "The Complete Italian Cookbook"},
                {Key.Last, "Mazzocco"}, {Key.First, "Manuela"}, {Key.Region, Region.Italy}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Food for Life"},
                {Key.Last, "Batmanglij"}, {Key.First, "Najmieh"}, {Key.Region, Region.Persia}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "The New Orleans Kitchen"},
                {Key.Last, "Devillier"}, {Key.First, "Justin"}, {Key.Region, Region.US}
            }));

            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Rodney Scott's World of BBQ"},
                {Key.Last, "Scott"}, {Key.First, "Rodney"}, {Key.Region, Region.US}
            }));

            // HowTo
            catalogue.Add(new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.HowTo}, {Key.Title, "On Writing: A Memoir of the Craft"},
                {Key.Last, "King"}, {Key.First, "Stephen"}, {Key.Subject, Subject.Writing}
            }));
        }

        static void Search(Catalogue catalogue, Attributes target)
        {
            Console.WriteLine($"\nFind {target}");
            var matches = catalogue.Find(target);

            if (matches.Count == 0) Console.WriteLine("No matches.");
            else
            {
                Console.WriteLine("Matches:");
                foreach (var book in matches) Console.WriteLine($"  {book}");
            }
        }

        static void Test(Catalogue catalogue)
        {
            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "Life of Pi"},
                {Key.Last, "Martel"}, {Key.First, "Yann"},
                {Key.Year, 2003}, {Key.Genre, Genre.Adventure}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Last, "KING"}, {Key.Genre, Genre.Horror}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Title, "1984"},
                {Key.Last, "Orwell"}, {Key.First, "George"}, {Key.Genre, Genre.Classics}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Year, 1960}, {Key.Genre, Genre.Romance}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Year, 1960}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}, {Key.Genre, Genre.SciFi}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Fiction}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Title, "Mastering the Art of French Cooking"},
                {Key.Last, "Child"}, {Key.First, "Julia"}, {Key.Region, Region.France}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Region, Region.China}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Last, "Leung"}, {Key.Region, Region.Mexico}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Kind, Kind.Cookbook}, {Key.Last, "Scott"}, {Key.First, "Rodney"}
            }));

            Search(catalogue, new Attributes(new Dictionary<Key, object> {
                {Key.Last, "King"}
            }));
        }
    }
}