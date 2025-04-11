using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Services;

public interface IAuthorService
{
    //Task CreateBookAsync(AuthorDto author);
    Task CreateAuthorsAsync(List<AuthorDto> author);
    //Task<List<Author>> GetAllAuthorsAsync();
    //Task<Author> GetAuthorByIdAsync(int bookId);
    //Task<Author> GetAuthorByNameAsync(string name);
}


public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
{
    public async Task CreateAuthorsAsync(List<AuthorDto> authors)
    {

        var newAuthors = authors.Select(autor => new Author(autor.Name, autor.Surname)).ToList();


        await authorRepository.AddAuthorsAsync(newAuthors);
    }
}