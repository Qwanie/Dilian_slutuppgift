using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the database context
        using var context = new LibraryContext();
        
        // Infinite loop to display the menu until the user chooses to exit
        while (true)
        {
            Console.WriteLine("\nBibliotekssystem");
            Console.WriteLine("1. Lägg till författare");
            Console.WriteLine("2. Lägg till bok");
            Console.WriteLine("3. Visa alla böcker och författare");
            Console.WriteLine("4. Registrera boklån");
            Console.WriteLine("5. Visa alla lån");
            Console.WriteLine("6. Avsluta");
            
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
                    return; // Exit the application
            }
        }
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
}