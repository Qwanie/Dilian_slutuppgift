public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
}