using Library.Core.Entities;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Factories;

public static class BookFactory
{
    public static Book CreateBook(BookDto bookDto)
    {
        ArgumentNullException.ThrowIfNull(bookDto);
        return new Book.Builder()
            .SetName(bookDto.Name)
            .SetPagesCount(bookDto.PagesCount)
            .Build();
    }
}