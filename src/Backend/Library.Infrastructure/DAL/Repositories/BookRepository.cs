using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class BookRepository(LibraryDbContext context) : IBookRepository
{
    public async Task<List<Book>> GetAllBooksAsync() => await context.Books
        .Include(b => b.Authors)
        .Include(c => c.Category)
        .Include(p => p.Publisher)
        .ToListAsync();

    public async Task AddBookAsync(Book book) => 
        await context.Books.AddAsync(book);

    public async Task AddBooksAsync(IEnumerable<Book> books) => 
        await context.Books.AddRangeAsync(books);

    public async Task<Book?> GetBookByIdAsync(Guid? id) =>
        await context.Books
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(c => c.Category)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Book?> GetBookByNameAsync(string name) =>
        await context.Books
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(c => c.Category)
            .FirstOrDefaultAsync(b => b.Name == name);

    public void UpdateBook(Book book) => 
        context.Books.Update(book);

    public IQueryable<Book> QueryAsNoTracking() =>
        context.Books.AsNoTracking();
    
    public async Task<List<Borrow>> GetBorrowBooksWithUsersAsync() => await context.Borrows
        .Include(u => u.User)
        .Include(b => b.Book)
        .ThenInclude(a => a.Authors)
        .AsNoTracking()
        .ToListAsync();
    
    public async Task<bool> ExistsAsync(string name, List<Guid> authorIds, Guid? excludedBookId)
    {
        return await context.Books
            .AsNoTracking()
            .AnyAsync(book =>
                book.Id != excludedBookId &&
                book.Name == name &&
                book.Authors != null &&
                book.Authors
                    .Select(x => x.Id)
                    .OrderBy(x => x)
                    .SequenceEqual(authorIds.OrderBy(x => x)));
    }
}