namespace ConsoleApp1
{
    public class Book
    {
        public string Title { get; set; }
        public bool IsCheckedOut { get; set; }
        public string? BorrowedBy { get; set; }

        public Book(string title)
        {
            Title = title;
            IsCheckedOut = false;
            BorrowedBy = null;
        }
    }

    public static class Program
    {
        private const string PleaseEnterValidInput = "Please enter a valid input.";
        public static void Main()
        {
            var books = new List<Book>();
            bool isRunning = true;

            while (isRunning)
            {
                PrintMenu();
                string? choice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choice))
                {
                    Console.WriteLine(PleaseEnterValidInput);
                    continue;
                }

                switch (choice)
                {
                    case "1":
                        AddBook(books);
                        break;
                    case "2":
                        RemoveBook(books);
                        break;
                    case "3":
                        DisplayBooks(books);
                        break;
                    case "4":
                        SearchBook(books);
                        break;
                    case "5":
                        BorrowBook(books);
                        break;
                    case "6":
                        ReturnBook(books);
                        break;
                    case "7":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine(PleaseEnterValidInput);
                        break;
                }
            }

            Console.WriteLine("Exiting the program.");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("\nWhat do you want to do today? (Enter your choice 1-7)");
            Console.WriteLine("1. Add a Book");
            Console.WriteLine("2. Remove a Book");
            Console.WriteLine("3. Display List of Books");
            Console.WriteLine("4. Search for a Book");
            Console.WriteLine("5. Borrow a Book");
            Console.WriteLine("6. Return a Book");
            Console.WriteLine("7. Exit");
            Console.WriteLine();
        }

        public static void AddBook(List<Book> books)
        {
            Console.WriteLine("Enter book title:");
            string? input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(PleaseEnterValidInput);
                input = Console.ReadLine();
            }

            // Check if the book already exists (optional)
            if (books.Any(b => string.Equals(b.Title, input, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Book '{input}' already exists in the library.");
                return;
            }

            books.Add(new Book(input));
            Console.WriteLine($"Book '{input}' has been added.");
        }

        public static void RemoveBook(List<Book> books)
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Book list is empty.");
                return;
            }

            Console.WriteLine("Enter the title of the book to remove:");
            string? bookToRemove = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(bookToRemove))
            {
                Console.WriteLine(PleaseEnterValidInput);
                bookToRemove = Console.ReadLine();
            }

            Book? book = books.FirstOrDefault(b => string.Equals(b.Title, bookToRemove, StringComparison.OrdinalIgnoreCase));
            if (book != null)
            {
                books.Remove(book);
                Console.WriteLine($"Book '{bookToRemove}' has been removed.");
            }
            else
            {
                Console.WriteLine($"Book '{bookToRemove}' not found.");
            }
        }

        public static void DisplayBooks(List<Book> books)
        {
            Console.WriteLine("Current list of books:");
            int count = 0;
            foreach (var book in books)
            {
                if (!book.IsCheckedOut)
                {
                    Console.WriteLine($"{++count}. {book.Title}");
                }
            }
            if (count == 0)
            {
                Console.WriteLine("No books to display.");
            }
        }

        public static void SearchBook(List<Book> books)
        {
            // Prompt the user to input a book title to search for
            Console.WriteLine("Enter the title of the book to search:");
            string? searchTerm = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine(PleaseEnterValidInput);
                searchTerm = Console.ReadLine();
            }

            bool found = false;
            int position = 1;
            foreach (var book in books)
            {
                if (!book.IsCheckedOut && book.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    // If the book is found, display a message indicating it is available
                    Console.WriteLine($"Found: {book.Title} at position {position}");
                    found = true;
                }
                position++;
            }
            if (!found)
            {
                // If the book is not found, display a message that it’s not in the collection
                Console.WriteLine($"No book found with title containing '{searchTerm}'.");
            }
        }

        public static void BorrowBook(List<Book> books)
        {
            Console.WriteLine("Enter your username:");
            string? username = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine(PleaseEnterValidInput);
                username = Console.ReadLine();
            }

            // Limit the number of books to 3 at a time
            int borrowedCount = books.Count(b => b.BorrowedBy == username && b.IsCheckedOut);
            if (borrowedCount >= 3)
            {
                Console.WriteLine("You have already borrowed the maximum of 3 books. Please return a book before borrowing another.");
                return;
            }

            Console.WriteLine("Enter the title of the book to borrow:");
            string? bookToBorrow = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(bookToBorrow))
            {
                Console.WriteLine(PleaseEnterValidInput);
                bookToBorrow = Console.ReadLine();
            }

            Book? book = books.FirstOrDefault(b =>
                string.Equals(b.Title, bookToBorrow, StringComparison.OrdinalIgnoreCase) && !b.IsCheckedOut);

            // Add a feature that flags a book as checked out
            if (book != null)
            {
                book.IsCheckedOut = true;
                book.BorrowedBy = username;
                Console.WriteLine($"Book '{book.Title}' has been borrowed by {username}.");
            }
            else
            {
                Console.WriteLine($"Book '{bookToBorrow}' not found or already borrowed.");
            }
        }

        public static void ReturnBook(List<Book> books)
        {
            Console.WriteLine("Enter your username:");
            string? username = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine(PleaseEnterValidInput);
                username = Console.ReadLine();
            }

            // Add a feature that tracks how many books a user has borrowed
            var borrowedBooks = books.Where(b => b.BorrowedBy == username && b.IsCheckedOut).ToList();
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("You have not borrowed any books.");
                return;
            }

            Console.WriteLine("Books you have borrowed:");
            foreach (var borrowedBook in borrowedBooks)
            {
                Console.WriteLine(borrowedBook.Title);
            }

            Console.WriteLine("Enter the title of the book to return:");
            string? bookToReturn = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(bookToReturn))
            {
                Console.WriteLine(PleaseEnterValidInput);
                bookToReturn = Console.ReadLine();
            }

            Book? book = books.FirstOrDefault(b =>
                b.BorrowedBy == username &&
                b.IsCheckedOut &&
                string.Equals(b.Title, bookToReturn, StringComparison.OrdinalIgnoreCase));

            // If the book is checked out, remove the checked-out flag to check the book in
            if (book != null)
            {
                book.IsCheckedOut = false;
                book.BorrowedBy = null;
                Console.WriteLine($"Book '{book.Title}' has been returned by {username}.");
            }
            else
            {
                Console.WriteLine($"You have not borrowed the book '{bookToReturn}'.");
            }
        }
    }
}