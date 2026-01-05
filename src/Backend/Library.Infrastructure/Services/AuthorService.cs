using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Exceptions;

namespace Library.Infrastructure.Services;

public interface IAuthorService
{
    Task<Guid> CreateAuthorAsync(AuthorDto author);
    Task CreateAuthorsAsync(List<AuthorDto> author);
    Task<List<AuthorDto>> GetAuthorsAsync();
}

public class AuthorService(IAuthorRepository authorRepository, IAuthorReadRepository authorReadRepository) : IAuthorService
{
    public async Task<Guid> CreateAuthorAsync(AuthorDto authors)
    {
        var existingAuthors = await authorReadRepository.GetAuthorByNameAsync(authors.Name, authors.Surname);
        if (existingAuthors != null)
        {
            throw new AuthorAlreadyExistsException();
        }

        var newAuthors = new Author(authors.Name, authors.Surname);
        await authorRepository.AddAuthorAsync(newAuthors);
        return newAuthors.Id;
    }
    
    public async Task CreateAuthorsAsync(List<AuthorDto> authors)
    {
        var newAuthors = authors
            .Select(autor => new Author(autor.Name, autor.Surname))
            .ToList();
        
        await authorRepository.AddAuthorsAsync(newAuthors);
    }

    public async Task<List<AuthorDto>> GetAuthorsAsync()
    {
        var authorsList = await authorReadRepository.GetAuthorsAsync();
        return authorsList
            .Select(x => new AuthorDto()
            {
                Id = x.Id,
                Name = x.Name ?? string.Empty,
                Surname = x.Surname ?? string.Empty
            })
            .OrderBy(x => x.Name)
            .ToList();
    }
}