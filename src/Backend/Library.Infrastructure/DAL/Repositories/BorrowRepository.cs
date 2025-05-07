using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class BorrowRepository(LibraryDbContext context) : IBorrowRepository
{
    public async Task<List<Borrow>> GetBorrowsBookByUserIdAsync(Guid userId)
    {
        return await context.Borrows
            .Include(x => x.Book)
            .Include(x => x.User)
            .Where(x => x.UserId == userId).ToListAsync();
    }
    
    public async Task<User?> GetUserByBorrowedBookIdAsync(Guid bookId)
    {
        return await context.Borrows
            .Include(u => u.User)
            .Where(b => b.BookId == bookId)
            .Select(x => x.User)
            .FirstOrDefaultAsync();
    }

    public async Task AddBorrowAsync(Borrow borrow)
    {
        await context.Borrows.AddAsync(borrow);
        await context.SaveChangesAsync();
    }

    public async Task RemoveBorrowAsync(Borrow borrow)
    {
        context.Borrows.Remove(borrow);
        await context.SaveChangesAsync();
    }

    public async Task<Borrow?> GetBorrowByUserIdAndBookIdAsync(Guid userId, Guid bookId)
    {
        return await context.Borrows
            .FirstOrDefaultAsync(x => 
                x.BookId == bookId && x.UserId == userId);
    }
    
    public Task<bool> ExistBorrowAsync(Guid bookId) =>
        context.Borrows
            .AsNoTracking()
            .AnyAsync(x =>
                x.BookId == bookId
            );
}