using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public sealed class Book : BaseClass
{
    public Name Name { get; private set; }
    public int PagesCount { get; private set; }
    public string Description { get; private set; }
    public Guid PublisherId { get; private set; }
    public Publisher? Publisher { get; private set; }
    public string ISBN { get; private set; }
    public string YearOfRelease { get; private set; }
    public Category? Category { get; private set; }
    public Guid? CategoryId { get; private set; }
    public ICollection<Author>? Authors { get; private set; }
    public List<Borrow> Borrows { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public bool IsDeleted { get; private set; }
    public string Code { get; set; }

    public Book()
    {
    }

    public Book(string name, List<Author> authors, Publisher publisher, Category category, string isbn, string description, int pagesCount, string yearOfRelease, string code)
    {
        SetName(name);
        SetAuthors(authors);
        SetPublisher(publisher);
        SetCategory(category);
        SetDescription(description);
        SetIsbn(isbn);
        SetPagesCount(pagesCount);
        SetYearOfRelease(yearOfRelease);
        SetCode(code);
    }

    public void SetName(Name name)
    {
        Name = name ?? throw new NullReferenceException(nameof(name));
    }

    public void SetCategory(Category category)
    {
        Category = category ?? throw new NullReferenceException(nameof(category));
    }

    public void SetPublisher(Publisher publisher)
    {
        Publisher = publisher ?? throw new NullReferenceException(nameof(publisher));
    }

    public void SetAuthors(List<Author> authors)
    {
        if (authors == null || authors.Count == 0)
        {
            throw new NullReferenceException(nameof(authors));
        }

        Authors = authors;
    }

    public void SetAvailable(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
    
    public void SetDescription(string description)
    {
        Description = description ?? throw new NullReferenceException(nameof(description));
    }
    
    public void SetIsbn(string isbn)
    {
        ISBN = isbn ?? throw new NullReferenceException(nameof(isbn));
    }

    public void SetYearOfRelease(string yearOfRelease)
    {
        YearOfRelease = yearOfRelease ?? throw new NullReferenceException(nameof(yearOfRelease));
    }
    
    public void SetPagesCount(int pagesCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pagesCount);

        PagesCount = pagesCount;
    }
    
    public void SetSoftDelete()
    {
        IsDeleted = true;
    }
    public void SetCode(string code)
    {
        Code = code ?? throw new NullReferenceException(nameof(code));
    }
}