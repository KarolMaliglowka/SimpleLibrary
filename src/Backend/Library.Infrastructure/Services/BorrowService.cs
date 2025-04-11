using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Services;

public interface IBorrowService
{
    Task CreateBorrow(BorrowDto borrowDto);
    Task DeleteBorrow(BorrowDto borrowDto);
}

public class BorrowService(
    IBorrowRepository borrowRepository,
    IUserRepository userRepository,
    IBookRepository bookRepository,
    IBookService bookService,
    IArchiveService archiveService)
    : IBorrowService
{
    public async Task CreateBorrow(BorrowDto borrowDto)
    {
        var user = await userRepository.GetUserByIdAsync(borrowDto.UserId);
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }
        var book = await bookRepository.GetBookByIdAsync(borrowDto.BookId);
        if (book == null)
        {
            throw new NullReferenceException("Book not found");
        }
        var newBorrow = new Borrow(user, book, DateTime.UtcNow);
        await borrowRepository.AddBorrowAsync(newBorrow);
        await bookService.SetBookAsBorrowed(book.Id, true);
    }

    public async Task DeleteBorrow(BorrowDto borrowDto)
    {
        var borrowToRemove = await borrowRepository
            .GetBorrowByUserIdAndBookIdAsync(borrowDto.UserId, borrowDto.BookId);
        if (borrowToRemove == null)
        {
            throw new NullReferenceException("Borrow not found");
        }
        await borrowRepository.RemoveBorrowAsync(borrowToRemove);
        await bookService.SetBookAsBorrowed(borrowDto.BookId, false);
        await archiveService.AddArchive(borrowToRemove);
    }
}