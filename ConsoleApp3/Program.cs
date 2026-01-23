using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp3
{
    public static class Program
    {
        private static readonly List<Book> Books = new();
        private static int _nextId = 1;

        public static void Main(string[] args)
        {
            SeedSampleBooks();

            PrintMessage("Welcome to the book catalog demo.");

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("1) List all books");
                Console.WriteLine("2) Add a book");
                Console.WriteLine("3) Search by title/author");
                Console.WriteLine("4) Show lowest price");
                Console.WriteLine("5) Exit");

                string choice = ReadRequiredString("Choose an option: ");

                switch (choice)
                {
                    case "1":
                        PrintBooks(Books);
                        break;
                    case "2":
                        AddBook();
                        break;
                    case "3":
                        SearchBooks();
                        break;
                    case "4":
                        ShowLowestPriceBook();
                        break;
                    case "5":
                        isRunning = false;
                        break;
                    default:
                        PrintMessage("Invalid selection. Try again.");
                        break;
                }
            }
        }

        private static void AddBook()
        {
            string title = ReadRequiredString("Title: ");
            string author = ReadRequiredString("Author: ");
            decimal price = ReadPositiveDecimal("Price: ");
            string? notes = ReadOptionalString("Notes (optional): ");

            Books.Add(new Book
            {
                Id = _nextId++,
                Title = title,
                Author = author,
                Price = price,
                Notes = notes
            });

            PrintMessage("Book added.");
        }

        private static void SearchBooks()
        {
            string query = ReadRequiredString("Search text: ");

            List<Book> results = Books
                .Where(book => ContainsIgnoreCase(book.Title, query)
                    || ContainsIgnoreCase(book.Author, query))
                .ToList();

            DisplaySearchResults(query, results);
        }

        private static bool ContainsIgnoreCase(string? source, string? value)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void DisplaySearchResults(string query, List<Book> results)
        {
            Console.WriteLine();
            Console.WriteLine($"Results for '{query}':");
            PrintBooks(results);
        }

        private static void PrintBooks(List<Book> books)
        {
            if (books.Count == 0)
            {
                PrintMessage("No books found.");
                return;
            }

            foreach (Book book in books)
            {
                Console.WriteLine($"#{book.Id}: {book.Title} by {book.Author} - ${book.Price:F2}");
                if (!string.IsNullOrWhiteSpace(book.Notes))
                {
                    Console.WriteLine($"  Notes: {book.Notes}");
                }
            }
        }

        private static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static decimal? ReadOptionalDecimal(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            return decimal.TryParse(input, out decimal value) ? value : null;
        }

        private static string? ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
        }

        private static decimal ReadPositiveDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal value) && value > 0)
                {
                    return value;
                }

                PrintMessage("Please enter a positive number.");
            }
        }

        private static string ReadRequiredString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                PrintMessage("This field is required.");
            }
        }

        private static void SeedSampleBooks()
        {
            Books.AddRange(new[]
            {
                new Book { Id = _nextId++, Title = "Clean Code", Author = "Robert C. Martin", Price = 29.99m, Notes = "Software craftsmanship" },
                new Book { Id = _nextId++, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 25.50m, Notes = "Classic advice" },
                new Book { Id = _nextId++, Title = "C# in Depth", Author = "Jon Skeet", Price = 39.95m }
            });
        }

        private static bool TryGetBookById(int id, out Book? book)
        {
            book = Books.FirstOrDefault(b => b.Id == id);
            return book is not null;
        }

        private static void ShowLowestPriceBook()
        {
            Book? cheapest = Books.OrderBy(book => book.Price).FirstOrDefault();

            if (cheapest is null)
            {
                PrintMessage("No books available.");
                return;
            }

            Console.WriteLine($"Lowest price: {cheapest.Title} - ${cheapest.Price:F2}");
        }
    }

    public sealed class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }
}
