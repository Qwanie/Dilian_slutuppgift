using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the database context
        using var context = new LibraryContext();

        // Seed initial data if database is empty
        if (!context.Authors.Any() && !context.Books.Any() && !context.BookLoans.Any())
        {
            SeedData(context);
        }
        
        // Infinite loop to display the menu until the user chooses to exit
        while (true)
        {
            Console.WriteLine("\nBibliotekssystem");
            Console.WriteLine("1. Lägg till författare");
            Console.WriteLine("2. Lägg till bok");
            Console.WriteLine("3. Visa alla böcker och författare");
            Console.WriteLine("4. Registrera boklån");
            Console.WriteLine("5. Visa alla lån");
            Console.WriteLine("6. Ta bort författare");
            Console.WriteLine("7. Ta bort bok"); 
            Console.WriteLine("8. Ta bort lån");
            Console.WriteLine("9. Visa utlånade böcker");
            Console.WriteLine("10. Avsluta");
            Console.WriteLine("11. Radera all data"); 
            Console.WriteLine("12. Lista alla böcker av en författare");
            Console.WriteLine("13. Lista alla författare av en bok");
            Console.WriteLine("14. Visa lånehistorik"); 
            
            // Read the user's menu choice
            var choice = Console.ReadLine();
            
            // Execute the action based on user's choice
            switch (choice)
            {
                case "1":
                    AddAuthor(context); // Add a new author
                    break;
                case "2":
                    AddBook(context);   // Add a new book
                    break;
                case "3":
                    ShowAllBooks(context);  // Display all books and their authors
                    break;
                case "4":
                    RegisterLoan(context);  // Register a new book loan
                    break;
                case "5":
                    ShowAllLoans(context);  // Display all book loans
                    break;
                case "6":
                    RemoveAuthor(context);  // Remove an author
                    break;
                case "7":
                    RemoveBook(context);    // Remove a book
                    break;
                case "8":
                    RemoveLoan(context);    // Remove a loan
                    break;
                case "9":
                    ShowLoanedBooks(context); // Display all currently loaned books
                    break;
                case "10":
                    return; // Exit the application
                case "11":
                    DeleteAllData(context); // Delete all data
                    break;
                case "12":
                    ListBooksByAuthor(context); // List books and authors
                    break;
                case "13":
                    ListAuthorsByBook(context); // List authors by book
                    break;
                case "14":
                    ShowLoanHistory(context); // Visa lånehistorik
                    break;
                default:
                    Console.WriteLine("\nOgiltig inmatning! Var god välj ett nummer mellan 1-14."); // Error handling
                    break;
            }
        }
    }

    // Method to seed initial data
    static void SeedData(LibraryContext context)
    {
        var random = new Random();
        var firstNames = new[] { "Johan", "Maria", "Erik", "Anna", "Karl", "Eva", "Anders", "Lisa", "Per", "Sofia" };
        var lastNames = new[] { "Andersson", "Nilsson", "Karlsson", "Svensson", "Pettersson", "Larsson", "Berg", "Åberg", "Holm", "Lindberg" };
        var bookTitles = new[] { "Historien om", "Äventyret med", "Mysteriet med", "Sagan om", "Berättelsen om" };
        var bookSubjects = new[] { "Draken", "Riddaren", "Prinsessan", "Trollkarlen", "Skatten", "Slottet", "Skogen", "Havet", "Staden", "Resan" };

        // Create 10 authors
        for (int i = 0; i < 10; i++)
        {
            var author = new Author 
            { 
                Name = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}" 
            };
            context.Authors.Add(author);
        }
        context.SaveChanges();

        var authors = context.Authors.ToList();

        // Create 100 books with random authors
        for (int i = 0; i < 100; i++)
        {
            var book = new Book
            {
                Title = $"{bookTitles[random.Next(bookTitles.Length)]} {bookSubjects[random.Next(bookSubjects.Length)]}",
                ISBN = $"{random.Next(100, 999)}-{random.Next(1000000, 9999999)}"
            };

            // Add 1-3 random authors to each book
            var authorCount = random.Next(1, 4);
            var randomAuthors = authors.OrderBy(x => random.Next()).Take(authorCount);
            foreach (var author in randomAuthors)
            {
                book.Authors.Add(author);
            }

            context.Books.Add(book);
        }
        context.SaveChanges();

        var books = context.Books.ToList();
        var borrowerNames = new[] { "Adam", "Beatrice", "Carl", "Diana", "Edward", "Fiona", "Gustav", "Helena", "Isak", "Julia" };

        // Create 100 loans with random books and borrowers
        for (int i = 0; i < 100; i++)
        {
            var daysAgo = random.Next(-30, 30); // Loans from 30 days ago to 30 days in future
            var loanDate = DateTime.Now.AddDays(daysAgo);
            
            var loan = new BookLoan
            {
                BookId = books[random.Next(books.Count)].Id,
                BorrowerName = borrowerNames[random.Next(borrowerNames.Length)],
                LoanDate = loanDate,
                ReturnDate = loanDate.AddDays(14)
            };
            context.BookLoans.Add(loan);
        }
        context.SaveChanges();

        Console.WriteLine("Database seeded with 100 authors, books, and loans!");
    }

    // Method to add a new author to the database
    static void AddAuthor(LibraryContext context)
    {
        Console.WriteLine("Ange författarens namn:");   // Prompt for author's name
        var name = Console.ReadLine();  // Read the author's name
        
        // Create a new Author object with the provided name
        var author = new Author { Name = name };
        context.Authors.Add(author);    // Add the author to the context
        context.SaveChanges();  // Save changes to the database
        
        Console.WriteLine("Författare tillagd!");   // Confirm addition
    }

    // Method to add a new book to the database
    static void AddBook(LibraryContext context) // Prompt for book title
    {
        Console.WriteLine("Ange bokens titel:");  // Read the book title
        var title = Console.ReadLine();
        
        Console.WriteLine("Ange ISBN:");    // Prompt for ISBN
        var isbn = Console.ReadLine();  // Read the ISBN
        
        // Create a new Book object with the provided title and ISBN
        var book = new Book { Title = title, ISBN = isbn };
        
        Console.WriteLine("Ange ID för författare (separera med komma om flera):"); // Prompt for author IDs
        var authors = context.Authors.ToList();
        System.Console.WriteLine("Välj från listan:");
        foreach (var author in authors)
        {
            System.Console.WriteLine($"Author: {author.Name} har Id {author.Id}"); 
        }
        var authorIds = Console.ReadLine().Split(',').Select(int.Parse);    // Read the input string
        
        foreach (var id in authorIds)
        {
            var author = context.Authors.Find(id);
            if (author != null)
            {
                book.Authors.Add(author);
            }
        }
        
        context.Books.Add(book);    // Add the book to the context
        context.SaveChanges();  // Save changes to the database
        
        Console.WriteLine("Bok tillagd!"); // Confirm addition
    }

    // Method to display all books along with their authors
    static void ShowAllBooks(LibraryContext context)
    {
        // Retrieve all books and include their related authors
        var books = context.Books.Include(b => b.Authors).ToList();
        
        // Iterate through each book and display its details
        foreach (var book in books)
        {
            Console.WriteLine("\n======================");
            Console.WriteLine($"  Titel:      {book.Title}");
            Console.WriteLine($"  ISBN:       {book.ISBN}");
            Console.WriteLine($"  Författare: {string.Join(", ", book.Authors.Select(a => a.Name))}");
            Console.WriteLine("======================");
        }
    }

    // Method to register a new book loan
    static void RegisterLoan(LibraryContext context)
    {
        Console.WriteLine("Ange bokens ID:");   // Prompt for book ID
        var bookId = int.Parse(Console.ReadLine());
        
        Console.WriteLine("Ange låntagarens namn:");    // Prompt for borrower's name
        var borrowerName = Console.ReadLine();  // Read borrower's name
        
        // Create a new BookLoan object with the provided details
        var loan = new BookLoan
        {
            BookId = bookId,
            BorrowerName = borrowerName,
            LoanDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(14)   // Set return date to 14 days from now
        };
        
        context.BookLoans.Add(loan);    // Add the loan to the context
        context.SaveChanges();  // Save changes to the database
        
        Console.WriteLine("Lån registrerat!");  // Confirm registration
    }

    // Method to display all book loans
    static void ShowAllLoans(LibraryContext context)
    {
        // Retrieve all book loans and include the related book details
        var loans = context.BookLoans.Include(l => l.Book).ToList();
        
        // Iterate through each loan and display its details
        foreach (var loan in loans)
        {
            Console.WriteLine($"\nBok: {loan.Book.Title}");
            Console.WriteLine($"Låntagare: {loan.BorrowerName}");
            Console.WriteLine($"Utlåningsdatum: {loan.LoanDate:d}");
            Console.WriteLine($"Återlämningsdatum: {loan.ReturnDate:d}");
        }
    }

    // Method to remove an author from the database
    static void RemoveAuthor(LibraryContext context)
    {
        Console.WriteLine("Ange författarens ID som ska tas bort:");
        var authors = context.Authors.ToList();
        foreach (var author in authors)
        {
            Console.WriteLine($"ID: {author.Id}, Namn: {author.Name}");
        }
        
        if (int.TryParse(Console.ReadLine(), out int authorId))
        {
            var author = context.Authors.Find(authorId);
            if (author != null)
            {
                context.Authors.Remove(author);
                // Get all books associated with this author
                var authorBooks = context.Books
                    .Include(b => b.Authors)
                    .Where(b => b.Authors.Any(a => a.Id == authorId))
                    .ToList();

                // Remove each book associated with the author
                foreach (var book in authorBooks)
                {
                    context.Books.Remove(book);
                }
                context.SaveChanges();
                Console.WriteLine("Författaren har tagits bort!");
            }
            else
            {
                Console.WriteLine("Författaren hittades inte.");
            }
        }
    }

    // Method to remove a book from the database
    static void RemoveBook(LibraryContext context)
    {
        Console.WriteLine("Ange bokens ID som ska tas bort:");
        var books = context.Books.ToList();
        foreach (var book in books)
        {
            Console.WriteLine($"ID: {book.Id}, Titel: {book.Title}");
        }
        
        if (int.TryParse(Console.ReadLine(), out int bookId))
        {
            var book = context.Books.Find(bookId);
            if (book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
                Console.WriteLine("Boken har tagits bort!");
            }
            else
            {
                Console.WriteLine("Boken hittades inte.");
            }
        }
    }

    // Method to remove a loan from the database
    static void RemoveLoan(LibraryContext context)
    {
        Console.WriteLine("Ange lånets ID som ska tas bort:");
        var loans = context.BookLoans.Include(l => l.Book).ToList();
        foreach (var loan in loans)
        {
            Console.WriteLine($"ID: {loan.Id}, Bok: {loan.Book.Title}, Låntagare: {loan.BorrowerName}");
        }
        
        if (int.TryParse(Console.ReadLine(), out int loanId))
        {
            var loan = context.BookLoans.Find(loanId);
            if (loan != null)
            {
                context.BookLoans.Remove(loan);
                context.SaveChanges();
                Console.WriteLine("Lånet har tagits bort!");
            }
            else
            {
                Console.WriteLine("Lånet hittades inte.");
            }
        }
    }

    // Method to show currently loaned books
    static void ShowLoanedBooks(LibraryContext context)
    {
        var currentLoans = context.BookLoans
            .Include(l => l.Book)
            .Where(l => l.ReturnDate > DateTime.Now)
            .ToList();

        if (currentLoans.Any())
        {
            Console.WriteLine("\nAktuella lån:");
            foreach (var loan in currentLoans)
            {
                Console.WriteLine($"\nBok: {loan.Book.Title}");
                Console.WriteLine($"Låntagare: {loan.BorrowerName}");
                Console.WriteLine($"Återlämnas: {loan.ReturnDate:d}");
            }
        }
        else
        {
            Console.WriteLine("\nInga aktiva lån just nu.");
        }
    }

    // Ny metod för att radera all data
    static void DeleteAllData(LibraryContext context)
    {
        Console.WriteLine("Är du säker på att du vill radera all data? (ja/nej)");
        var confirmation = Console.ReadLine();
        if (confirmation?.ToLower() == "ja")
        {
            try
            {
                // Börja med att ta bort relaterade poster för att undvika referensfel
                context.BookLoans.RemoveRange(context.BookLoans);
                context.Books.RemoveRange(context.Books);
                context.Authors.RemoveRange(context.Authors);
                context.SaveChanges();
                Console.WriteLine("All data har raderats.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Åtgärd avbruten.");
        }
    }

    // Ny metod för att lista alla böcker av en specifik författare
    static void ListBooksByAuthor(LibraryContext context)
    {
        Console.WriteLine("Ange författarens ID eller namn:");
        var input = Console.ReadLine();

        if (int.TryParse(input, out int authorId))
        {
            var author = context.Authors
                                .Include(a => a.Books)
                                .FirstOrDefault(a => a.Id == authorId);
            if (author != null)
            {
                Console.WriteLine($"\nFörfattare: {author.Name}");
                if (author.Books.Any())
                {
                    Console.WriteLine("Böcker:");
                    foreach (var book in author.Books)
                    {
                        Console.WriteLine($"- {book.Title} (ISBN: {book.ISBN})");
                    }
                }
                else
                {
                    Console.WriteLine("Denna författare har inga böcker listade.");
                }
            }
            else
            {
                Console.WriteLine("Författaren hittades inte.");
            }
        }
        else
        {
            var authors = context.Authors
                                 .Where(a => a.Name.Contains(input))
                                 .Include(a => a.Books)
                                 .ToList();

            if (authors.Any())
            {
                foreach (var author in authors)
                {
                    Console.WriteLine($"\nFörfattare: {author.Name}");
                    if (author.Books.Any())
                    {
                        Console.WriteLine("Böcker:");
                        foreach (var book in author.Books)
                        {
                            Console.WriteLine($"- {book.Title} (ISBN: {book.ISBN})");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Denna författare har inga böcker listade.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Ingen författare matchar den angivna inmatningen.");
            }
        }
    }

    // Ny metod för att lista alla författare av en specifik bok
    static void ListAuthorsByBook(LibraryContext context)
    {
        Console.WriteLine("Ange bokens ID eller titel:");
        var input = Console.ReadLine();

        if (int.TryParse(input, out int bookId))
        {
            var book = context.Books
                              .Include(b => b.Authors)
                              .FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                Console.WriteLine($"\nBok: {book.Title} (ISBN: {book.ISBN})");
                if (book.Authors.Any())
                {
                    Console.WriteLine("Författare:");
                    foreach (var author in book.Authors)
                    {
                        Console.WriteLine($"- {author.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("Denna bok har inga författare listade.");
                }
            }
            else
            {
                Console.WriteLine("Boken hittades inte.");
            }
        }
        else
        {
            var books = context.Books
                               .Where(b => b.Title.Contains(input))
                               .Include(b => b.Authors)
                               .ToList();

            if (books.Any())
            {
                foreach (var book in books)
                {
                    Console.WriteLine($"\nBok: {book.Title} (ISBN: {book.ISBN})");
                    if (book.Authors.Any())
                    {
                        Console.WriteLine("Författare:");
                        foreach (var author in book.Authors)
                        {
                            Console.WriteLine($"- {author.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Denna bok har inga författare listade.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Ingen bok matchar den angivna inmatningen.");
            }
        }
    }

    // Ny metod för att visa lånehistorik
    static void ShowLoanHistory(LibraryContext context)
    {
        Console.WriteLine("\nLånehistorik:");
        
        // Hämtar alla lån där ReturnDate är tidigare än eller lika med dagens datum
        var loanHistory = context.BookLoans
                                 .Include(l => l.Book)
                                 .Where(l => l.ReturnDate <= DateTime.Now)
                                 .OrderByDescending(l => l.LoanDate)
                                 .ToList();

        if (loanHistory.Any())
        {
            foreach (var loan in loanHistory)
            {
                Console.WriteLine("\n======================");
                Console.WriteLine($"Bok: {loan.Book.Title} (ISBN: {loan.Book.ISBN})");
                Console.WriteLine($"Låntagare: {loan.BorrowerName}");
                Console.WriteLine($"Utlåningsdatum: {loan.LoanDate:d}");
                Console.WriteLine($"Återlämningsdatum: {loan.ReturnDate:d}");
                Console.WriteLine("======================");
            }
        }
        else
        {
            Console.WriteLine("Inga tidigare lån finns registrerade.");
        }
    }
}