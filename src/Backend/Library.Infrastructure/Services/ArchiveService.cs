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

        var result = string.Join(", ", book.Authors.Select(author =>
            $"{author.Name} {author.Surname}").ToArray());
        var archive = new Archive(
            book.Id, book.Name, result, user.Id, user.FullName, borrow.BorrowDate, DateTime.UtcNow
        );
        await archiveRepository.AddArchive(archive);
    }
}