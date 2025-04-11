using Library.Core.Entities;

namespace Library.Core.Repositories;

public interface IBookRepository
{
    Task AddBooksAsync(IEnumerable<Book> books);
    Task<Book?> GetBookByIdAsync(Guid id);
    Task AddBookAsync(Book book);
    Task<List<Book>> GetAllAsync();
    Task UpdateBook(Book book);
    Task<Book?> GetBookByNameAsync(string name);
}