using Library.Core.Entities;
using Library.Core.Repositories;

namespace Library.Infrastructure.Services;

public interface IArchiveService
{
    Task AddArchive(Borrow borrow);
}

public class ArchiveService(IBookService bookService, IUserService userService, IArchiveRepository archiveRepository)
    : IArchiveService
{
    public async Task AddArchive(Borrow borrow)
    {
        var user = await userService.GetUserById(borrow.UserId);
        var book = await bookService.GetBookByIdAsync(borrow.BookId);

        var bookAuthors = string.Join(", ", book.Authors.Select(author =>
            $"{author.Name} {author.Surname}").ToArray());

        var archive = new ArchiveBuilder()
            .SetBookId(book.Id)
            .SetBookName(book.Name)
            .SetAuthors(bookAuthors)
            .SetUserId(user.Id)
            .SetUserFullName(user.FullName)
            .SetBorrowDate(borrow.BorrowDate)
            .SetReturnDate(DateTime.UtcNow)
            .Build();

        await archiveRepository.AddArchive(archive);
    }
}