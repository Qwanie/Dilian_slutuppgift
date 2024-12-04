using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using var context = new LibraryContext();
        
        while (true)
        {
            Console.WriteLine("\nBibliotekssystem");
            Console.WriteLine("1. Lägg till författare");
            Console.WriteLine("2. Lägg till bok");
            Console.WriteLine("3. Visa alla böcker och författare");
            Console.WriteLine("4. Registrera boklån");
            Console.WriteLine("5. Visa alla lån");
            Console.WriteLine("6. Avsluta");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    AddAuthor(context);
                    break;
                case "2":
                    AddBook(context);
                    break;
                case "3":
                    ShowAllBooks(context);
                    break;
                case "4":
                    RegisterLoan(context);
                    break;
                case "5":
                    ShowAllLoans(context);
                    break;
                case "6":
                    return;
            }
        }
    }

    static void AddAuthor(LibraryContext context)
    {
        Console.WriteLine("Ange författarens namn:");
        var name = Console.ReadLine();
        
        var author = new Author { Name = name };
        context.Authors.Add(author);
        context.SaveChanges();
        
        Console.WriteLine("Författare tillagd!");
    }

    static void AddBook(LibraryContext context)
    {
        Console.WriteLine("Ange bokens titel:");
        var title = Console.ReadLine();
        
        Console.WriteLine("Ange ISBN:");
        var isbn = Console.ReadLine();
        
        var book = new Book { Title = title, ISBN = isbn };
        
        Console.WriteLine("Ange ID för författare (separera med komma om flera):");
        var authorIds = Console.ReadLine().Split(',').Select(int.Parse);
        
        foreach (var id in authorIds)
        {
            var author = context.Authors.Find(id);
            if (author != null)
            {
                book.Authors.Add(author);
            }
        }
        
        context.Books.Add(book);
        context.SaveChanges();
        
        Console.WriteLine("Bok tillagd!");
    }

    static void ShowAllBooks(LibraryContext context)
    {
        var books = context.Books.Include(b => b.Authors).ToList();
        
        foreach (var book in books)
        {
            Console.WriteLine($"\nTitel: {book.Title}");
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine("Författare: " + string.Join(", ", book.Authors.Select(a => a.Name)));
        }
    }

    static void RegisterLoan(LibraryContext context)
    {
        Console.WriteLine("Ange bokens ID:");
        var bookId = int.Parse(Console.ReadLine());
        
        Console.WriteLine("Ange låntagarens namn:");
        var borrowerName = Console.ReadLine();
        
        var loan = new BookLoan
        {
            BookId = bookId,
            BorrowerName = borrowerName,
            LoanDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(14)
        };
        
        context.BookLoans.Add(loan);
        context.SaveChanges();
        
        Console.WriteLine("Lån registrerat!");
    }

    static void ShowAllLoans(LibraryContext context)
    {
        var loans = context.BookLoans.Include(l => l.Book).ToList();
        
        foreach (var loan in loans)
        {
            Console.WriteLine($"\nBok: {loan.Book.Title}");
            Console.WriteLine($"Låntagare: {loan.BorrowerName}");
            Console.WriteLine($"Utlåningsdatum: {loan.LoanDate:d}");
            Console.WriteLine($"Återlämningsdatum: {loan.ReturnDate:d}");
        }
    }
}