using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public class Book : BaseClass
{
    protected Book()
    {
    }

    public Book(Name name, List<Author> authors, int pagesCount, string description, string isbn,
        string yearOfRelease, Publisher publisher, Category category)
    {
        Id = Guid.NewGuid();
        SetName(name);
        PagesCount = pagesCount;
        Description = description;
        ISBN = isbn;
        YearOfRelease = yearOfRelease;
        Authors = authors;
        CreatedAt = DateTime.UtcNow;
        Publisher = publisher;
        Category = category;
    }

    public Name Name { get; set; }
    public int PagesCount { get; set; }
    public string Description { get; set; }
    public Guid PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
    public string ISBN { get; set; }
    public string YearOfRelease { get; set; }
    public Category? Category { get; set; }
    public Guid? CategoryId { get; set; }
    public ICollection<Author>? Authors { get; set; }
    public List<Borrow> Borrows { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsBorrowed { get; set; }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            throw new ArgumentException("Name cannot be empty. It requires minimum 3 characters.");
        }
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
}