// Define the Book entity
public class Book
{
    // Primary key
    public int Id { get; set; }

    // Book title
    public string Title { get; set; }

    // Book ISBN
    public string ISBN { get; set; }

    // Navigation property for related authors
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
}