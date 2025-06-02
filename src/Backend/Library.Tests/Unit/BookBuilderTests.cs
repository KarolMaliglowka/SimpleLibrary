using Library.Core.Entities;
using Library.Core.Builders;
using Library.Core.Exceptions;
using Library.Core.ValueObjects;

public class BookBuilderTests
{
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
    [InlineData("Effective C#")]
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
        const int pageCount = 100;
        
        // Act
        bookBuilder.SetPagesCount(pageCount);
        var book = bookBuilder.Build();
    
        // Assert
        Assert.Equal(pageCount, book.PagesCount);
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
        var authors = new List<Author> { new Author ("John", "Doe" ) };

        // Act
        bookBuilder.SetAuthors(authors);
        var book = bookBuilder.Build();

        // Assert
        Assert.NotNull(book.Authors);
        Assert.Equal(authors, book.Authors);
    }
}