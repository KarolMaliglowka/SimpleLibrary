using Library.Application.DTO;
using Library.Core;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

/// <summary>
/// Interfejs serwisu odpowiedzialnego za operacje związane z autorami.
/// </summary>
public interface IAuthorService
{
    /// <summary>
    /// Tworzy nowego autora w systemie.
    /// </summary>
    /// <param name="author">Dane autora w postaci DTO.</param>
    /// <returns>Id utworzonego autora.</returns>
    Task<Guid> CreateAuthorAsync(AuthorDto author);

    /// <summary>
    /// Tworzy wielu autorów na podstawie przekazanej listy.
    /// </summary>
    /// <param name="author">Lista autorów do zapisania.</param>
    /// <returns>Operacja asynchroniczna.</returns>
    Task CreateAuthorsAsync(List<AuthorDto> author);

    /// <summary>
    /// Pobiera listę wszystkich autorów.
    /// </summary>
    /// <returns>Lista autorów w postaci AuthorDto.</returns>
    Task<List<AuthorDto>> GetAuthorsAsync();
    Task<List<Dictionary<Guid, string>>> GetAuthorsDictionaryAsync();
    Task UpdateAuthorAsync(AuthorDto author);
}

/// <summary>
/// Implementacja serwisu odpowiedzialnego za zarządzanie autorami.
/// </summary>
public class AuthorService(IAuthorRepository authorRepository, IAuthorReadRepository authorReadRepository, IUnitOfWork unitOfWork, ILogger<AuthorService> logger)
    : IAuthorService
{
    /// <summary>
    /// Tworzy nowego autora w bazie danych.
    /// </summary>
    /// <param name="authors">Dane autora.</param>
    /// <returns>Id nowo utworzonego autora.</returns>
    /// <exception cref="AuthorAlreadyExistsException">
    /// Rzucany gdy autor o podanym imieniu i nazwisku już istnieje.
    /// </exception>
    public async Task<Guid> CreateAuthorAsync(AuthorDto authors)
    {
        var existingAuthor = await authorReadRepository.GetAuthorAsync(authors.Surname, authors.Name);
        if (existingAuthor != null)
        {
            throw new AlreadyExistsException("Author", existingAuthor.FullName);
        }

        var newAuthors = new Author(authors.Name, authors.Surname);
        await authorRepository.AddAuthorAsync(newAuthors);
        await unitOfWork.SaveChangesAsync();
        return newAuthors.Id;
    }

    /// <summary>
    /// Dodaje wielu autorów do bazy danych.
    /// </summary>
    /// <param name="authors">Lista autorów w postaci DTO.</param>
    /// <returns>Operacja asynchroniczna zapisu autorów.</returns>
    public async Task CreateAuthorsAsync(List<AuthorDto> authors)
    {
        var newAuthors = authors
            .Select(autor => new Author(autor.Name, autor.Surname))
            .ToList();

        authorRepository.AddAuthors(newAuthors);
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Pobiera autorów z repozytorium odczytowego,
    /// mapuje encje na DTO oraz sortuje wynik po imieniu.
    /// </summary>
    /// <returns>Lista autorów w postaci AuthorDto.</returns>
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

    public async Task<List<Dictionary<Guid, string>>> GetAuthorsDictionaryAsync()
    {
        var authorsList = await authorReadRepository.GetAuthorsAsync();
        return authorsList
            .OrderBy(x => x.FullName)
            .Select(x => new Dictionary<Guid, string>
            {
                [x.Id] = x.FullName
            })
            .ToList();
    }
    
    public async Task DeleteAuthorAsync(Guid id)
    {
        var authorExist = await authorReadRepository.GetAuthorByIdAsync(id);
        if (authorExist == null)
        {
            logger.Log(LogLevel.Error, "Author with id: '{AuthorId}' not found", id);
            throw new NotFoundException("Author", $" with id: {id}");
        }

        var authorsList = await authorReadRepository.GetAuthorsAsync();
        var isAuthorForSomeBook = authorsList.Any(x =>
            (x.Name.Value.ToLower()).Equals(authorExist.Name.Value,
                StringComparison.CurrentCultureIgnoreCase)); //TODO do poprawy
        if (isAuthorForSomeBook)
        {
            throw new IsInUseException("Author", authorExist.Name);
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
        existingAuthor.SetName(author.Surname);

        await authorRepository.UpdateAuthorAsync(existingAuthor);
        await unitOfWork.SaveChangesAsync();
    }
}