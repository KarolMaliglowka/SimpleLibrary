using Library.Core.Entities;

namespace Library.Core.Repositories;

public interface IBorrowRepository
{
    Task<List<Borrow>> GetBorrowsBookByUserIdAsync(Guid userId);
    Task<User?> GetUserByBorrowedBookIdAsync(Guid bookId);
    Task AddBorrowAsync(Borrow borrow);
    void RemoveBorrow(Borrow borrow);
    Task<Borrow?> GetBorrowByUserIdAndBookIdAsync(Guid userId, Guid bookId);
    Task<bool> ExistBorrowAsync(Guid bookId);
    Task<Borrow?> GetBorrowByIdAsync(Guid id);
}