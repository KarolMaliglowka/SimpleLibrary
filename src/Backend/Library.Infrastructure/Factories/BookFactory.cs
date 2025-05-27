using Library.Core.Entities;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Factories;

public static class BookFactory
{
    public static Book CreateBook(
        BookDto bookDto,
        List<Author>? authors,
        Publisher? publisher,
        Category? category
    )
    {
        ArgumentNullException.ThrowIfNull(bookDto);
        ArgumentNullException.ThrowIfNull(authors);
        return new Book.Builder()
            .SetName(bookDto.Name)
            .SetPagesCount(bookDto.PagesCount)
            .SetDescription(bookDto.Description)
            .SetIsbn(bookDto.Isbn)
            .SetYearOfRelease(bookDto.YearOfRelease)
            .SetAuthors(authors)
            .SetPublisher(publisher)
            .SetCategory(category)
            .Build();
    }
}