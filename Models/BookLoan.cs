// Define the BookLoan entity
public class BookLoan
{
    // Primary key
    public int Id { get; set;}

    // Foreign key to Book
    public int BookId { get; set;}

    // Navigation property to Book
    public Book Book { get; set;}

    // Date when the book was loaned
    public DateTime LoanDate { get; set;}

    // Expected return date
    public DateTime ReturnDate { get; set;}

    // Name of the borrower
    public string BorrowerName { get; set;}
}    