public class BookLoan
{
    public int Id { get; set;}
    public int BookId { get; set;}
    public Book Book { get; set;}
    public DateTime LoanDate { get; set;}
    public DateTime ReturnDate { get; set;}
    public string BorrowerName { get; set;}
}    