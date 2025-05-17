using Library.Core.ValueObjects;

namespace Library.Core.Entities;
public sealed class Publisher : BaseClass
{
    private readonly List<Book> _books = new();

    public Publisher(Name name)
    {
        Id = Guid.NewGuid();
        SetPublisher(name);
        CreatedAt = DateTime.UtcNow;
    }
    public Name Name { get; set; }
    public IEnumerable<Book> Books => _books.AsReadOnly();
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public void SetPublisher(string publisher)
    {
        if (string.IsNullOrWhiteSpace(publisher) || publisher.Length < 2)
        {
            throw new ArgumentException("Publisher cannot be empty. It requires minimum 3 characters.");
        }

        Name = publisher;
        UpdatedAt = DateTime.UtcNow;
    }
}