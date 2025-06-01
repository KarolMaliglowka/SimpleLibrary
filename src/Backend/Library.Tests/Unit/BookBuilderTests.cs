using Library.Core.Entities;
using Library.Core.Builders;
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

    [Fact]
    public void SetName_ShouldSetBookName()
    {
        // Arrange
        var bookBuilder = new BookBuilder();
        var name = new Name("Effective C#");

        // Act
        bookBuilder.SetName(name);
        var book = bookBuilder.Build();

        // Assert
        Assert.Equal(name, book.Name);
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