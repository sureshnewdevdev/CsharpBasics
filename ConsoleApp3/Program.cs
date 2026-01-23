using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace ConsoleApp3;

public static class Program
{
    private static readonly List<dynamic> Books = new();
    private static int _nextId = 1;

    public static void Main(string[] args)
    {
        SeedSampleBooks();
        ShowMainMenu();
    }

    private static void ShowMainMenu()
    {
        bool exitRequested = false;
        while (!exitRequested)
        {
            Console.Clear();
            Console.WriteLine("=== Book Library Management System ===");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. User");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowAdminMenu();
                    break;
                case "2":
                    ShowUserMenu();
                    break;
                case "3":
                    exitRequested = true;
                    break;
                default:
                    PrintMessage("Invalid choice. Press ENTER to try again.");
                    break;
            }
        }
    }

    private static void ShowAdminMenu()
    {
        bool backRequested = false;
        while (!backRequested)
        {
            Console.Clear();
            Console.WriteLine("--- Admin Menu ---");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Update Book");
            Console.WriteLine("3. Delete Book");
            Console.WriteLine("4. View All Books");
            Console.WriteLine("5. Back");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    UpdateBook();
                    break;
                case "3":
                    DeleteBook();
                    break;
                case "4":
                    ViewAllBooks();
                    break;
                case "5":
                    backRequested = true;
                    break;
                default:
                    PrintMessage("Invalid choice. Press ENTER to try again.");
                    break;
            }
        }
    }

    private static void ShowUserMenu()
    {
        bool backRequested = false;
        while (!backRequested)
        {
            Console.Clear();
            Console.WriteLine("--- User Menu ---");
            Console.WriteLine("1. Browse Books");
            Console.WriteLine("2. Search Book by Name");
            Console.WriteLine("3. Search Book by Publisher");
            Console.WriteLine("4. View Highest Price Book");
            Console.WriteLine("5. View Lowest Price Book");
            Console.WriteLine("6. Back");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ViewAllBooks();
                    break;
                case "2":
                    SearchByName();
                    break;
                case "3":
                    SearchByPublisher();
                    break;
                case "4":
                    ShowHighestPriceBook();
                    break;
                case "5":
                    ShowLowestPriceBook();
                    break;
                case "6":
                    backRequested = true;
                    break;
                default:
                    PrintMessage("Invalid choice. Press ENTER to try again.");
                    break;
            }
        }
    }

    private static void AddBook()
    {
        Console.Clear();
        Console.WriteLine("--- Add Book ---");

        dynamic book = new ExpandoObject();
        book.Id = _nextId++;
        book.Name = ReadRequiredString("Book Name: ");
        book.Author = ReadRequiredString("Author: ");
        book.Publisher = ReadRequiredString("Publisher: ");
        book.Price = ReadPositiveDecimal("Price: ");

        Books.Add(book);
        PrintMessage($"Book '{book.Name}' added successfully.");
    }

    private static void UpdateBook()
    {
        Console.Clear();
        Console.WriteLine("--- Update Book ---");
        if (!TryGetBookById(out dynamic? book))
        {
            PrintMessage("Book not found. Press ENTER to return.");
            return;
        }

        Console.WriteLine("Press ENTER to keep existing values.");
        book.Name = ReadOptionalString($"Name ({book.Name}): ", book.Name);
        book.Author = ReadOptionalString($"Author ({book.Author}): ", book.Author);
        book.Publisher = ReadOptionalString($"Publisher ({book.Publisher}): ", book.Publisher);
        book.Price = ReadOptionalDecimal($"Price ({book.Price}): ", book.Price);

        PrintMessage("Book updated successfully.");
    }

    private static void DeleteBook()
    {
        Console.Clear();
        Console.WriteLine("--- Delete Book ---");
        if (!TryGetBookById(out dynamic? book))
        {
            PrintMessage("Book not found. Press ENTER to return.");
            return;
        }

        Books.Remove(book);
        PrintMessage("Book deleted successfully.");
    }

    private static void ViewAllBooks()
    {
        Console.Clear();
        Console.WriteLine("--- Book List ---");

        if (Books.Count == 0)
        {
            PrintMessage("No books available.");
            return;
        }

        PrintBooks(Books);
        PrintMessage("End of list.");
    }

    private static void SearchByName()
    {
        Console.Clear();
        Console.WriteLine("--- Search by Name ---");
        string term = ReadRequiredString("Enter book name: ");

        List<dynamic> matches = Books
            .Where(book => ContainsIgnoreCase(book.Name, term))
            .ToList();

        DisplaySearchResults(matches, "name");
    }

    private static void SearchByPublisher()
    {
        Console.Clear();
        Console.WriteLine("--- Search by Publisher ---");
        string term = ReadRequiredString("Enter publisher name: ");

        List<dynamic> matches = Books
            .Where(book => ContainsIgnoreCase(book.Publisher, term))
            .ToList();

        DisplaySearchResults(matches, "publisher");
    }

    private static void ShowHighestPriceBook()
    {
        Console.Clear();
        Console.WriteLine("--- Highest Price Book ---");
        if (Books.Count == 0)
        {
            PrintMessage("No books available.");
            return;
        }

        dynamic book = Books.OrderByDescending(item => (decimal)item.Price).First();
        PrintBooks(new List<dynamic> { book });
        PrintMessage("Highest price book displayed.");
    }

    private static void ShowLowestPriceBook()
    {
        Console.Clear();
        Console.WriteLine("--- Lowest Price Book ---");
        if (Books.Count == 0)
        {
            PrintMessage("No books available.");
            return;
        }

        dynamic book = Books.OrderBy(item => (decimal)item.Price).First();
        PrintBooks(new List<dynamic> { book });
        PrintMessage("Lowest price book displayed.");
    }

    private static bool TryGetBookById(out dynamic? book)
    {
        int id = ReadPositiveInt("Enter Book ID: ");
        book = Books.FirstOrDefault(item => item.Id == id);
        return book is not null;
    }

    private static void PrintBooks(IEnumerable<dynamic> books)
    {
        Console.WriteLine("ID  Name                     Author                  Publisher               Price");
        Console.WriteLine("------------------------------------------------------------------------------------");
        foreach (dynamic book in books)
        {
            Console.WriteLine(
                $"{book.Id,-3} {Truncate(book.Name, 24),-24} {Truncate(book.Author, 22),-22} {Truncate(book.Publisher, 20),-20} {book.Price,8:C}");
        }
        Console.WriteLine("------------------------------------------------------------------------------------");
    }

    private static void DisplaySearchResults(List<dynamic> matches, string criteria)
    {
        if (matches.Count == 0)
        {
            PrintMessage($"No books found for that {criteria}.");
            return;
        }

        PrintBooks(matches);
        PrintMessage($"{matches.Count} book(s) found.");
    }

    private static void SeedSampleBooks()
    {
        AddSampleBook("Clean Code", "Robert C. Martin", "Prentice Hall", 39.99m);
        AddSampleBook("The Pragmatic Programmer", "Andrew Hunt", "Addison-Wesley", 42.50m);
        AddSampleBook("Refactoring", "Martin Fowler", "Addison-Wesley", 47.25m);
    }

    private static void AddSampleBook(string name, string author, string publisher, decimal price)
    {
        dynamic book = new ExpandoObject();
        book.Id = _nextId++;
        book.Name = name;
        book.Author = author;
        book.Publisher = publisher;
        book.Price = price;
        Books.Add(book);
    }

    private static string ReadRequiredString(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(input));

        return input.Trim();
    }

    private static string ReadOptionalString(string prompt, string currentValue)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? currentValue : input.Trim();
    }

    private static decimal ReadPositiveDecimal(string prompt)
    {
        decimal value;
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a valid non-negative price.");
        }
    }

    private static decimal ReadOptionalDecimal(string prompt, decimal currentValue)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return currentValue;
        }

        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
        {
            return value;
        }

        Console.WriteLine("Invalid price. Keeping existing value.");
        return currentValue;
    }

    private static int ReadPositiveInt(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a valid positive integer.");
        }
    }

    private static bool ContainsIgnoreCase(string source, string term)
    {
        return source.Contains(term, StringComparison.OrdinalIgnoreCase);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Length <= maxLength ? value : value[..(maxLength - 3)] + "...";
    }

    private static void PrintMessage(string message)
    {
        Console.WriteLine();
        Console.WriteLine(message);
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}
