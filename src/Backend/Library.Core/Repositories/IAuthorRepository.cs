using Library.Core.Entities;

namespace Library.Core.Repositories;

public interface IAuthorRepository
{
    // Task<List<Author>> GetAuthorsAsync();
    // Task<Author?> GetAuthorByIdAsync(Guid id);
    // Task<Author?> GetAuthorByNameAsync(string name);
    // Task<List<Author>> GetAuthorBySurnameAsync(string surname);
    Task AddAuthorAsync(Author author);
    Task UpdateAuthorAsync(Author author);
    //Task<List<Author>> GetAuthorsWithBooksAsync();
    Task<bool> ExistAuthorAsync(Author author);
    Task DeleteAuthorAsync(Author author);
    Task AddAuthorsAsync(List<Author> author);
    //Task<Author?> GetAuthorAsync(string surname,string? name = null);
    Task<bool> ExistAuthorAsync(string name, string? surname = null);
}

public interface IAuthorReadRepository
{
        Task<List<Author>> GetAuthorsAsync();
        Task<Author?> GetAuthorByIdAsync(Guid id);
        Task<Author?> GetAuthorByNameAsync(string name);
        Task<List<Author>> GetAuthorBySurnameAsync(string surname);
        Task<List<Author>> GetAuthorsWithBooksAsync();
        Task<Author?> GetAuthorAsync(string surname,string? name = null);
    
}