using Library.Core.Builders;
using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.Exceptions;

namespace Library.Infrastructure.Services;

public interface IArchiveService
{
    Task AddArchive(Borrow borrow);
}

public class ArchiveService : IArchiveService
{
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IArchiveRepository _archiveRepository;

    public ArchiveService(
        IBookService bookService,
        IUserService userService,
        IArchiveRepository archiveRepository)
    {
        _bookService = bookService;
        _userService = userService;
        _archiveRepository = archiveRepository;
    }

    public async Task AddArchive(Borrow borrow)
    {
        ArgumentNullException.ThrowIfNull(borrow);

        var user = await _userService.GetUserById(borrow.UserId)
                   ?? throw new UserNotFoundException(borrow.UserId.ToString());

        var book = await _bookService.GetBookByIdAsync(borrow.BookId)
                   ?? throw new BookNotFoundException(borrow.BookId.ToString());

        var authors = book.Authors == null
            ? string.Empty
            : string.Join(", ", book.Authors.Select(a => $"{a.Name} {a.Surname}"));

        var archive = new ArchiveBuilder()
            .SetBookId(book.Id)
            .SetBookName(book.Name ?? string.Empty)
            .SetAuthors(authors)
            .SetUserId(user.Id)
            .SetUserFullName(user.FullName)
            .SetBorrowDate(borrow.BorrowDate)
            .SetReturnDate(DateTime.UtcNow)
            .Build();

        await _archiveRepository.AddArchive(archive);
    }
}