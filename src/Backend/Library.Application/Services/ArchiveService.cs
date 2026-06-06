using Library.Core;
using Library.Core.Builders;
using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Core.Exceptions;

namespace Library.Application.Services;

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
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArchiveRepository _archiveRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArchiveService"/> class.
    /// </summary>
    /// <param name="bookRepository">Repository used to retrieve book data.</param>
    /// <param name="userRepository">Repository used to retrieve user data.</param>
    /// <param name="archiveRepository">Repository used to persist archive records.</param>
    /// <param name="unitOfWork"></param>
    public ArchiveService(
        IArchiveRepository archiveRepository,
        IUnitOfWork unitOfWork,
        IBookRepository bookRepository,
        IUserRepository userRepository)
    {
        _archiveRepository = archiveRepository;
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
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

        var user = await _userRepository.GetUserByIdAsync(borrow.UserId)
                   ?? throw new UserNotFoundException(borrow.UserId.ToString());
        var book = await _bookRepository.GetBookByIdAsync(borrow.BookId)
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