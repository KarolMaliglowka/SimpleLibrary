namespace Library.Core.Entities;

public class Category : BaseClass
{
    
    private readonly List<Book> _books = [];
    public Category()
    {
    }
    
    public Category(string name)
    {
        Id = Guid.NewGuid();
        SetCategory(name);
        CreatedAt = DateTime.UtcNow;
    }
    
    public string Name { get; set; }
    public ICollection<Book> Books => _books.AsReadOnly();
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void SetCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category) || category.Length < 3)
        {
            throw new ArgumentException("Category cannot be empty. It requires minimum 4 characters.");
        }

        Name = category;
        UpdatedAt = DateTime.UtcNow;
    }
}