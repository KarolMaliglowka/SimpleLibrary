using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public sealed class Book : BaseClass
{
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

    public sealed class Builder
    {
        private readonly Book _book;

        public Builder()
        {
            _book = new Book
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
        }

        public Builder(Book existingBook)
        {
            _book = existingBook;
        }

        public Builder SetName(Name name)
        {
            ValidateInput(name.Value, "Name", 2);
            _book.Name = name;
            return this;
        }

        public Builder SetPagesCount(int pagesCount)
        {
            if (pagesCount > 0)
            {
                throw new Exception("Pages count can't be less than 0.");
            }

            _book.PagesCount = pagesCount;
            return this;
        }

        public Builder SetDescription(string description)
        {
            ValidateInput(description, "Description", 2);
            _book.Description = description;
            return this;
        }

        public Builder SetIsbn(string isbn)
        {
            ValidateInput(isbn, "ISBN", 2);
            _book.ISBN = isbn;
            return this;
        }

        public Builder SetYearOfRelease(string yearOfRelease)
        {
            ValidateInput(yearOfRelease, "YearOfRelease", 1);
            _book.YearOfRelease = yearOfRelease;
            return this;
        }

        public Builder SetAuthors(List<Author> authors)
        {
            ArgumentNullException.ThrowIfNull(authors);
            _book.Authors = authors;
            return this;
        }

        public Builder SetPublisher(Publisher publisher)
        {
            ArgumentNullException.ThrowIfNull(publisher);
            _book.Publisher = publisher;
            return this;
        }

        public Builder SetCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);
            _book.Category = category;
            return this;
        }

        public Book Build()
        {
            _book.UpdateTimestamp();
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

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}