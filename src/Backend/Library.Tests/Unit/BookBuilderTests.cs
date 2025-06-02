using Library.Core.Builders;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.ValueObjects;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Factories;

namespace Library.Tests.Unit;

public class BookBuilderTests
{
    private const string Description = "Advanced C# concepts";
    private const string Isbn = "978-1-23456-789-0";
    private const string YearOfRelease = "2023";
    private const int PagesCount = 500;
    private const string Name = "C# in Depth";

    [Fact]
    public void Build_ShouldCreateBookWithDefaultValues()
    {
        // Act
        var book = new BookBuilder().Build();

        // Assert
        Assert.NotNull(book);
        Assert.NotEqual(Guid.Empty, book.Id);
        Assert.NotNull(book.CreatedAt);
    }

    [Theory]
    [InlineData(Name)]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("E")]
    public void SetName_ShouldSetBookName(string? name)
    {
        var bookBuilder = new BookBuilder();

        if (name == null || name.Length < 2)
        {
            Assert.Throws<InvalidNameException>(() => bookBuilder.SetName(new Name(name)));
        }
        else
        {
            bookBuilder.SetName(name);
            var book = bookBuilder.Build();
            Assert.Equal(name, book.Name);
        }
    }

    [Fact]
    public void SetPagesCount_ShouldSetPagesCount()
    {
        // Arrange
        var bookBuilder = new BookBuilder();

        // Act
        bookBuilder.SetPagesCount(PagesCount);
        var book = bookBuilder.Build();

        // Assert
        Assert.Equal(PagesCount, book.PagesCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void SetPagesCount_ShouldThrowException(int pageCounts)
    {
        // Arrange
        var bookBuilder = new BookBuilder();

        // Act & Assert
        Assert.Throws<Exception>(() => bookBuilder.SetPagesCount(pageCounts));
    }

    [Fact]
    public void SetAuthors_ShouldSetBookAuthors()
    {
        // Arrange
        var bookBuilder = new BookBuilder();
        var authors = new List<Author> { new Author("John", "Doe") };

        // Act
        bookBuilder.SetAuthors(authors);
        var book = bookBuilder.Build();

        // Assert
        Assert.NotNull(book.Authors);
        Assert.Equal(authors, book.Authors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("E")]
    public void SetDescription_ShouldThrowException(string description)
    {
        var bookBuilder = new BookBuilder();

        if (string.IsNullOrEmpty(description) || description.Length < 2)
        {
            Assert.Throws<ArgumentException>(() => bookBuilder.SetDescription(description));
        }
        else
        {
            bookBuilder.SetDescription(Description);
            var book = bookBuilder.Build();
            Assert.NotNull(book.Description);
            Assert.Equal(Description, book.Description);
        }
    }

    [Fact]
    public void SetPublisher_ShouldSetBookPublisher()
    {
        // Arrange
        var bookBuilder = new BookBuilder();
        var publisher = PublisherFactory.CreatePublisher(new PublisherDto
        {
            Name = "PublisherName"
        });

        // Act
        bookBuilder.SetPublisher(publisher);
        var book = bookBuilder.Build();

        // Assert
        Assert.NotNull(book.Publisher);
        Assert.Equal(publisher, book.Publisher);
    }

    [Fact]
    public void SetCategory_ShouldSetBookCategory()
    {
        // Arrange
        var bookBuilder = new BookBuilder();
        var category = new Category("CategoryName");

        // Act
        bookBuilder.SetCategory(category);
        var book = bookBuilder.Build();

        // Assert
        Assert.NotNull(book.Category);
        Assert.Equal(category, book.Category);
    }

    [Fact]
    public void SetIsbn_ShouldSetBookIsbn()
    {
        // Arrange
        var bookBuilder = new BookBuilder();

        // Act
        bookBuilder.SetIsbn(Isbn);
        var book = bookBuilder.Build();

        // Assert
        Assert.NotNull(book.ISBN);
        Assert.Equal(Isbn, book.ISBN);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("E")]
    public void SetIsbn_ShouldThrowException(string isbn)
    {
        // Arrange
        var bookBuilder = new BookBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => bookBuilder.SetDescription(isbn));
    }

    [Fact]
    public void SetYearOfRelease_ShouldSetBookYearOfRelease()
    {
        // Arrange
        var bookBuilder = new BookBuilder();

        // Act
        bookBuilder.SetYearOfRelease(YearOfRelease);
        var book = bookBuilder.Build();

        // Assert
        Assert.NotNull(book.YearOfRelease);
        Assert.Equal(YearOfRelease, book.YearOfRelease);
    }

    [Fact]
    public void BookBuilder_ShouldInitializeFromExistingBook()
    {
        // Arrange
        var existingBook = new Book
        {
            Id = Guid.NewGuid(),
            Name = new Name(Name),
            PagesCount = PagesCount,
            Description = Description,
            ISBN = Isbn,
            YearOfRelease = YearOfRelease,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var bookBuilder = new BookBuilder(existingBook);
        var newBook = bookBuilder.Build();

        // Assert
        Assert.Equal(existingBook.Id, newBook.Id);
        Assert.Equal(existingBook.Name, newBook.Name);
        Assert.Equal(existingBook.PagesCount, newBook.PagesCount);
        Assert.Equal(existingBook.Description, newBook.Description);
        Assert.Equal(existingBook.ISBN, newBook.ISBN);
        Assert.Equal(existingBook.YearOfRelease, newBook.YearOfRelease);
        Assert.Equal(existingBook.CreatedAt, newBook.CreatedAt);
    }
}