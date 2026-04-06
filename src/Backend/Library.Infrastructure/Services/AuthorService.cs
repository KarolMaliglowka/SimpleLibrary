using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Exceptions;

namespace Library.Infrastructure.Services;

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
}

/// <summary>
/// Implementacja serwisu odpowiedzialnego za zarządzanie autorami.
/// </summary>
public class AuthorService(IAuthorRepository authorRepository, IAuthorReadRepository authorReadRepository) : IAuthorService
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
        var existingAuthors = await authorReadRepository.GetAuthorAsync(authors.Surname, authors.Name);
        if (existingAuthors != null)
        {
            throw new AuthorAlreadyExistsException();
        }

        var newAuthors = new Author(authors.Name, authors.Surname);
        await authorRepository.AddAuthorAsync(newAuthors);
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

        await authorRepository.AddAuthorsAsync(newAuthors);
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
}