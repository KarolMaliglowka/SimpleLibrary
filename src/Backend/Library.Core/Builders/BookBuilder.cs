using Library.Core.Entities;
using Library.Core.ValueObjects;

namespace Library.Core.Builders;

public sealed class BookBuilder
{
    private readonly Book _book;

    public BookBuilder()
    {
        _book = new Book
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public BookBuilder(Book existingBook)
    {
        _book = existingBook;
    }

    public BookBuilder SetName(Name name)
    {
        ValidateInput(name.Value, "Name", 2);
        _book.Name = name;
        return this;
    }

    public BookBuilder SetPagesCount(int pagesCount)
    {
        if (pagesCount <= 0)
        {
            throw new Exception("Pages count can't be less or equal 0.");
        }

        _book.PagesCount = pagesCount;
        return this;
    }

    public BookBuilder SetDescription(string description)
    {
        ValidateInput(description, "Description", 2);
        _book.Description = description;
        return this;
    }

    public BookBuilder SetIsbn(string isbn)
    {
        ValidateInput(isbn, "ISBN", 2);
        _book.ISBN = isbn;
        return this;
    }

    public BookBuilder SetYearOfRelease(string yearOfRelease)
    {
        ValidateInput(yearOfRelease, "YearOfRelease", 1);
        _book.YearOfRelease = yearOfRelease;
        return this;
    }

    public BookBuilder SetAuthors(List<Author> authors)
    {
        ArgumentNullException.ThrowIfNull(authors);
        _book.Authors = authors;
        return this;
    }

    public BookBuilder SetPublisher(Publisher publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);
        _book.Publisher = publisher;
        return this;
    }

    public BookBuilder SetCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        _book.Category = category;
        return this;
    }

    public Book Build()
    {
        _book.UpdatedAt = DateTime.UtcNow;
        return _book;
    }

    private static void ValidateInput(string input, string fieldName, int minLength = 1)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < minLength)
        {
            throw new ArgumentException(
                $"{fieldName} cannot be empty and must have at least {minLength} characters.");
        }
    }
}