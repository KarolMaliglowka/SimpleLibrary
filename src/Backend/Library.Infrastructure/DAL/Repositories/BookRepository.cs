using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class BookRepository(LibraryDbContext context) : IBookRepository
{
    public async Task<List<Book>> GetAllAsync() => await context.Books
        .Include(b => b.Authors)
        .Include(c => c.Category)
        .Include(p => p.Publisher)
        .AsNoTracking()
        .ToListAsync();

    public async Task AddBookAsync(Book book)
    {
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
    }

    public async Task AddBooksAsync(IEnumerable<Book> books)
    {
        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }

    public async Task<Book?> GetBookByIdAsync(Guid id) =>
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

    public async Task UpdateBook(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
    }
}