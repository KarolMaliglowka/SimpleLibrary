using Library.Core;
using Library.Core.Builders;
using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.Exceptions;

namespace Library.Infrastructure.Services;

/// <summary>
/// Service responsible for creating archive entries when a borrowed book is returned.
/// </summary>
public interface IArchiveService
{
    /// <summary>
    /// Creates an archive entry based on a borrow record.
    /// </summary>
    /// <param name="borrow">Borrow entity representing a book borrowing.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddArchive(Borrow borrow);
}

/// <summary>
/// Implementation of <see cref="IArchiveService"/> responsible for storing archive records.
/// </summary>
public class ArchiveService : IArchiveService
{
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IArchiveRepository _archiveRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArchiveService"/> class.
    /// </summary>
    /// <param name="bookService">Service used to retrieve book data.</param>
    /// <param name="userService">Service used to retrieve user data.</param>
    /// <param name="archiveRepository">Repository used to persist archive records.</param>
    /// <param name="unitOfWork"></param>
    public ArchiveService(
        IBookService bookService,
        IUserService userService,
        IArchiveRepository archiveRepository,
        IUnitOfWork unitOfWork)
    {
        _bookService = bookService;
        _userService = userService;
        _archiveRepository = archiveRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adds a new archive entry for a returned book.
    /// </summary>
    /// <param name="borrow">Borrow entity containing information about the borrowing event.</param>
    /// <exception cref="ArgumentNullException">Thrown when the borrow object is null.</exception>
    /// <exception cref="UserNotFoundException">Thrown when the user associated with the borrow record does not exist.</exception>
    /// <exception cref="BookNotFoundException">Thrown when the book associated with the borrow record does not exist.</exception>
    /// <returns>A task representing the asynchronous archive creation operation.</returns>
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
        await _unitOfWork.SaveChangesAsync();
    }
}