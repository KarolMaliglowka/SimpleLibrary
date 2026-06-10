using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.ValueObjects;

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
        var book = new Book();

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
        var book = new Book();

        if (name == null || name.Length < 2)
        {
            Assert.Throws<InvalidNameException>(() => book.SetName(new Name(name!)));
        }
        else
        {
            book.SetName(name);
            Assert.Equal(name, book.Name);
        }
    }

    [Fact]
    public void SetPagesCount_ShouldSetPagesCount()
    {
        // Arrange
        var book = new Book();

        // Act
        book.SetPagesCount(PagesCount);

        // Assert
        Assert.Equal(PagesCount, book.PagesCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void SetPagesCount_ShouldThrowException(int pageCounts)
    {
        // Arrange
        var book = new Book();

        // Act & Assert
        Assert.Throws<Exception>(() => book.SetPagesCount(pageCounts));
    }

    [Fact]
    public void SetAuthors_ShouldSetBookAuthors()
    {
        // Arrange
        var book = new Book();
        var authors = new List<Author> { new Author("John", "Doe") };

        // Act
        book.SetAuthors(authors);
        

        // Assert
        Assert.NotNull(book.Authors);
        Assert.Equal(authors, book.Authors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("E")]
    public void SetDescription_ShouldThrowException(string? description)
    {
        var book = new Book();

        if (string.IsNullOrEmpty(description) || description.Length < 2)
        {
            Assert.Throws<ArgumentException>(() => book.SetDescription(description!));
        }
        else
        {
            book.SetDescription(Description);
            Assert.NotNull(book.Description);
            Assert.Equal(Description, book.Description);
        }
    }

    [Fact]
    public void SetPublisher_ShouldSetBookPublisher()
    {
        // Arrange
        var book = new Book();
        var publisher = new Publisher("PublisherName"
        );

        // Act
        book.SetPublisher(publisher);

        // Assert
        Assert.NotNull(book.Publisher);
        Assert.Equal(publisher, book.Publisher);
    }

    [Fact]
    public void SetCategory_ShouldSetBookCategory()
    {
        // Arrange
        var book = new Book();
        var category = new Category("CategoryName");

        // Act
        book.SetCategory(category);

        // Assert
        Assert.NotNull(book.Category);
        Assert.Equal(category, book.Category);
    }

    [Fact]
    public void SetIsbn_ShouldSetBookIsbn()
    {
        // Arrange
        var book = new Book();

        // Act
        book.SetIsbn(Isbn);
        
        // Assert
        Assert.NotNull(book.ISBN);
        Assert.Equal(Isbn, book.ISBN);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("E")]
    public void SetIsbn_ShouldThrowException(string? isbn)
    {
        // Arrange
        var book = new Book();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => book.SetDescription(isbn!));
    }

    [Fact]
    public void SetYearOfRelease_ShouldSetBookYearOfRelease()
    {
        // Arrange
        var book = new Book();

        // Act
        book.SetYearOfRelease(YearOfRelease);

        // Assert
        Assert.NotNull(book.YearOfRelease);
        Assert.Equal(YearOfRelease, book.YearOfRelease);
    }

    // [Fact]
    // public void BookBuilder_ShouldInitializeFromExistingBook()
    // {
    //     // Arrange
    //     var existingBook = new Book
    //     {
    //         Name = new Name(Name),
    //         PagesCount = PagesCount,
    //         Description = Description,
    //         ISBN = Isbn,
    //         YearOfRelease = YearOfRelease,
    //         CreatedAt = DateTime.UtcNow
    //     };
    //
    //     // Act
    //     var bookBuilder = new BookBuilder(existingBook);
    //     var newBook = bookBuilder.Build();
    //
    //     // Assert
    //     Assert.Equal(existingBook.Id, newBook.Id);
    //     Assert.Equal(existingBook.Name, newBook.Name);
    //     Assert.Equal(existingBook.PagesCount, newBook.PagesCount);
    //     Assert.Equal(existingBook.Description, newBook.Description);
    //     Assert.Equal(existingBook.ISBN, newBook.ISBN);
    //     Assert.Equal(existingBook.YearOfRelease, newBook.YearOfRelease);
    //     Assert.Equal(existingBook.CreatedAt, newBook.CreatedAt);
    // }
}