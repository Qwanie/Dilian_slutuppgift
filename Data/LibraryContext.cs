using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books{ get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookLoan> BookLoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-2IAQ0EH\\SQLEXPRESS;Database=LibrarySystemDb_Dilian;Trusted_Connection=True;TrustServerCertificate=True;");
    } 
}