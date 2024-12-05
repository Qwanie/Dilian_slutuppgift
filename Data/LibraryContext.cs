using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    // DbSet for Books table
    public DbSet<Book> Books{ get; set; }

    // DbSet for Authors table
    public DbSet<Author> Authors { get; set; }

    // DbSet for BookLoans table
    public DbSet<BookLoan> BookLoans { get; set; }

    // Configure the database connection
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-2IAQ0EH\\SQLEXPRESS;Database=LibrarySystemDb_Dilian;Trusted_Connection=True;TrustServerCertificate=True;");
    } 
}