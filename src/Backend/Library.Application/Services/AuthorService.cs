using Library.Application.DTO;
using Library.Core;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

/// <summary>
/// Interface of the service responsible for operations related to authors.
/// </summary>
public interface IAuthorService
{
    /// <summary>
    /// Create a new author in the system.
    /// </summary>
    /// <param name="author">Author data in the form of DTO.</param>
    /// <returns>Created author id.</returns>
    Task<Guid> CreateAuthorAsync(AuthorDto author);
    /// <summary>
    /// Create multiple authors based on the list provided.
    /// </summary>
    /// <param name="author">List of authors to save.</param>
    Task CreateAuthorsAsync(List<AuthorDto> author);
    /// <summary>
    /// Pobiera listę wszystkich autorów.
    /// </summary>
    /// <returns>Lista autorów w postaci AuthorDto.</returns>
    Task<List<AuthorDto>> GetAuthorsAsync();
    Task<List<Dictionary<Guid, string>>> GetAuthorsDictionaryAsync();
    Task UpdateAuthorAsync(AuthorDto author);
    Task DeleteAuthorAsync(Guid id);
    Task<AuthorDto> GetAuthorsByIdAsync(Guid id);
}

/// <summary>
/// Implementation of a service responsible for managing authors.
/// </summary>
public class AuthorService(
    IAuthorRepository authorRepository,
    IAuthorReadRepository authorReadRepository,
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork,
    ILogger<AuthorService> logger)
    : IAuthorService
{
    /// <summary>
    /// Creates a new author in the database.
    /// </summary>
    /// <param name="authors">Authors data.</param>
    /// <returns>id of the newly created author</returns>
    /// <exception cref="AuthorAlreadyExistsException">
    /// Thrown when an author with the given name and surname already exists.
    /// </exception>
    public async Task<Guid> CreateAuthorAsync(AuthorDto author)
    {
        ArgumentNullException.ThrowIfNull(author);
        var existingAuthor = await authorReadRepository.GetAuthorAsync(author.Surname, author.Name);
        if (existingAuthor != null)
        {
            logger.Log(LogLevel.Error, "{AuthorName} '{Name}' already exists.", "Author",
                $"{author.Name} {author.Surname}");
            throw new AlreadyExistsException("Author", existingAuthor.FullName);
        }

        var newAuthors = new Author(author.Name, author.Surname);
        await authorRepository.AddAuthorAsync(newAuthors);
        await unitOfWork.SaveChangesAsync();
        return newAuthors.Id;
    }

    /// <summary>
    /// Adds multiple authors to the database.
    /// </summary>
    /// <param name="authors">List of authors in the form of DTO.</param>
    public async Task CreateAuthorsAsync(List<AuthorDto> authors)
    {
        var newAuthors = authors
            .Select(autor => new Author(autor.Name, autor.Surname))
            .ToList();

        authorRepository.AddAuthors(newAuthors);
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Gets authors from the reading repository,
    /// maps entities to DTOs and sorts the result by name.
    /// </summary>
    /// <returns>Author list in the form of AuthorDto.</returns>
    public async Task<List<AuthorDto>> GetAuthorsAsync()
    {
        var authorsList = await authorReadRepository.GetAuthorsAsync();
        return
        [
            .. authorsList
                .Select(x => new AuthorDto()
                {
                    Id = x.Id,
                    Name = x.Name ?? string.Empty,
                    Surname = x.Surname ?? string.Empty,
                    IsDeleted = x.IsDeleted
                })
                .OrderBy(x => x.Name)
        ];
    }

    public async Task<List<Dictionary<Guid, string>>> GetAuthorsDictionaryAsync()
    {
        var authorsList = await authorReadRepository.GetAuthorsAsync();
        return
        [
            .. authorsList
                .OrderBy(x => x.FullName)
                .Where(x => !x.IsDeleted)
                .Select(x => new Dictionary<Guid, string>
                {
                    [x.Id] = x.FullName
                })
        ];
    }

    public async Task DeleteAuthorAsync(Guid id)
    {
        var authorExist = await authorReadRepository.GetAuthorByIdAsync(id);

        if (authorExist == null)
        {
            logger.Log(
                LogLevel.Error,
                "Author with id: '{AuthorId}' not found",
                id);

            throw new NotFoundException("Author", $"{id}");
        }

        var booksList = await bookRepository.GetAllBooksAsync();

        var isAuthorForSomeBook = booksList.Any(book =>
            book.Authors.Any(author => author.Id == authorExist.Id));

        if (isAuthorForSomeBook)
        {
            throw new IsInUseException(
                "Author",
                $"{authorExist.Name} {authorExist.Surname}");
        }

        authorExist.SetSoftDelete();
        await authorRepository.UpdateAuthorAsync(authorExist);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAuthorAsync(AuthorDto author)
    {
        ArgumentNullException.ThrowIfNull(author);
        var existingAuthor = await authorReadRepository.GetAuthorByIdAsync(author.Id);
        if (existingAuthor == null)
        {
            logger.Log(LogLevel.Error, "Author with id: '{AuthorId}' not found", author.Id);
            throw new NotFoundException("Author", $" with id: {author.Id}");
        }

        existingAuthor.SetName(author.Name);
        existingAuthor.SetSurname(author.Surname);

        await authorRepository.UpdateAuthorAsync(existingAuthor);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<AuthorDto> GetAuthorsByIdAsync(Guid id)
    {
        var existingAuthor = await authorReadRepository.GetAuthorByIdAsync(id);
        if (existingAuthor != null)
            return new AuthorDto()
            {
                Id = existingAuthor.Id,
                Name = existingAuthor.Name ?? string.Empty,
                Surname = existingAuthor.Surname ?? string.Empty,
                IsDeleted = existingAuthor.IsDeleted
            };
        logger.Log(LogLevel.Error, "Author with id: '{AuthorId}' not found", existingAuthor!.Id);
        throw new NotFoundException("Author", $" with id: {existingAuthor.Id}");
    }
}