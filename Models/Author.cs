using System.Collections.Concurrent;

// Define the Author entity
public class Author
{
    // Primary key
    public int Id { get; set; }

    // Author's name
    public string Name { get; set; }

    // Navigation property for related books
    public ICollection<Book> Books { get; set; } = new List<Book>();
}