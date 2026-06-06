using Library.Application.DTO;
using Library.Core;
using Library.Core.Builders;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;
using Microsoft.Extensions.Logging;

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
    IArchiveRepository archiveRepository,
    IUnitOfWork unitOfWork,
    ILogger<IBorrowService> logger)
    : IBorrowService
{
    public async Task CreateBorrow(BorrowDto borrowDto)
    {
        var user = await userRepository.GetUserByIdAsync(borrowDto.UserId);
        
        if (user == null)
        {
            throw new UserNotFoundException($"{borrowDto.UserFullName} with id: {borrowDto.UserId}");
        }
        
        var book = await bookRepository.GetBookByIdAsync(borrowDto.BookId);
        
        if (book == null)
        {
            throw new BookNotFoundException($"{borrowDto.BookName} with id: {borrowDto.BookId}");
        }
        
        var newBorrow = new Borrow(user, book, DateTime.UtcNow);
        await borrowRepository.AddBorrowAsync(newBorrow);
        
        book.IsAvailable = false;
        bookRepository.UpdateBook(book);
        
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBorrow(Guid id)
    {
        var borrowToRemove = await borrowRepository.GetBorrowByIdAsync(id);
        
        if (borrowToRemove == null)
        {
            throw new BorrowNotFoundException(id);
        }
        
        borrowRepository.RemoveBorrow(borrowToRemove);
        
        var book = await bookRepository.GetBookByIdAsync(borrowToRemove.BookId);
        
        if (book == null)
        {
            logger.LogError("Book id: {id} not found", borrowToRemove.BookId);
            throw new BookNotFoundException(borrowToRemove.BookId.ToString());
        }

        book.IsAvailable = true;
        bookRepository.UpdateBook(book);
      
        var user = await userRepository.GetUserByIdAsync(borrowToRemove.UserId)
                   ?? throw new UserNotFoundException(borrowToRemove.UserId.ToString());

        var authors = book.Authors == null
            ? string.Empty
            : string.Join(", ", book.Authors.Select(a => $"{a.Name} {a.Surname}"));

        var archive = new ArchiveBuilder()
            .SetBookId(book.Id)
            .SetBookName(book.Name ?? string.Empty)
            .SetAuthors(authors)
            .SetUserId(user.Id)
            .SetUserFullName(user.FullName)
            .SetBorrowDate(borrowToRemove.BorrowDate)
            .SetReturnDate(DateTime.UtcNow)
            .Build();

        await archiveRepository.AddArchive(archive);
       
        await unitOfWork.SaveChangesAsync();
    }
}