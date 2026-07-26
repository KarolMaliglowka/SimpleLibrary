using Library.Core.ValueObjects;

namespace Library.Core.Entities;
public sealed class Publisher : BaseClass
{
    public Publisher()
    {
    }
    public Publisher(Name name)
    {
        SetPublisher(name);
    }

    private readonly List<Book> _books = [];

    public Name Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public IEnumerable<Book> Books => _books.AsReadOnly();
    
    public void SetPublisher(string publisher)
    {
        if (string.IsNullOrWhiteSpace(publisher) || publisher.Length < 3)
        {
            throw new ArgumentException("Publisher cannot be empty. It requires minimum 4 characters.");
        }

        Name = publisher;
    }
    public void SetSoftDelete()
    {
        IsDeleted = true;
    }
}