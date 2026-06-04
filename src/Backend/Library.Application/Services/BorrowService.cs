using Library.Application.DTO;
using Library.Core;
using Library.Core.Entities;
using Library.Core.Repositories;

namespace Library.Application.Services;

public interface IBorrowService
{
    Task CreateBorrow(BorrowDto borrowDto);
    Task DeleteBorrow(Guid id);
}

public class BorrowService(
    IBorrowRepository borrowRepository,
    IUserRepository userRepository,
    IBookRepository bookRepository,
    IBookService bookService,
    IArchiveService archiveService,
    IUnitOfWork unitOfWork)
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
        await bookService.SetBookAsBorrowed(book.Id, false);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBorrow(Guid id)
    {
        var borrowToRemove = await borrowRepository.GetBorrowByIdAsync(id);
        if (borrowToRemove == null)
        {
            throw new NullReferenceException("Borrow not found");
        }
        borrowRepository.RemoveBorrow(borrowToRemove);
        await bookService.SetBookAsBorrowed(borrowToRemove.BookId, true);
        await archiveService.AddArchive(borrowToRemove);
        await unitOfWork.SaveChangesAsync();
    }
}