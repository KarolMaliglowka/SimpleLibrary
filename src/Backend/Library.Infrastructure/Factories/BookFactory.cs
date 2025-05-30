using Library.Core.Builders;
using Library.Core.Entities;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Factories;

public static class BookFactory
{
    public static Book BuildBook(BookDto bookDto,
        List<Author>? authors,
        Publisher? publisher,
        Category? category, Book? currentBook = null)
    {
        ArgumentNullException.ThrowIfNull(bookDto);
        ArgumentNullException.ThrowIfNull(authors);
        ArgumentNullException.ThrowIfNull(publisher);
        ArgumentNullException.ThrowIfNull(category);
        return currentBook switch
        {
            null => new BookBuilder()
                .SetName(bookDto.Name)
                .SetPagesCount(bookDto.PagesCount)
                .SetDescription(bookDto.Description)
                .SetIsbn(bookDto.Isbn)
                .SetYearOfRelease(bookDto.YearOfRelease)
                .SetAuthors(authors)
                .SetPublisher(publisher)
                .SetCategory(category)
                .Build(),
            _ => new BookBuilder(currentBook)
                .SetName(bookDto.Name)
                .SetPagesCount(bookDto.PagesCount)
                .SetDescription(bookDto.Description)
                .SetIsbn(bookDto.Isbn)
                .SetYearOfRelease(bookDto.YearOfRelease)
                .SetAuthors(authors)
                .SetPublisher(publisher)
                .SetCategory(category)
                .Build()
        };
    }
}