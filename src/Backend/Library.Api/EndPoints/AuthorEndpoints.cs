using Library.Application.DTO;
using Library.Application.Services;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;


namespace Library.Api.EndPoints;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this WebApplication app)
    {
        app.MapGet("/authors", async (IAuthorService authorService) =>
            await authorService.GetAuthorsAsync() is { } author
                ? Results.Ok(author)
                : Results.NotFound());

        app.MapPost("/authors/create", async (AuthorDto author, IAuthorService authorService) =>
        {
            try
            {
                var id = await authorService.CreateAuthorAsync(author);
                return Results.Created($"/author/{id}", new { Id = id });
            }
            catch (AlreadyExistsException ex)
            {
                return Results.Content(ex.Message);
            }
        });

        app.MapPost("/authors/createMany", async (List<AuthorDto> authors, IAuthorService authorService) =>
        {
            await authorService.CreateAuthorsAsync(authors);
            return Results.Created();
        });

        app.MapPut("/authors/update", async (
            AuthorDto author,
            IAuthorReadRepository authorReadRepository,
            IAuthorService authorService
        ) =>
        {
            var authorInDb = await authorReadRepository.GetAuthorByIdAsync(author.Id);
            if (authorInDb == null)
            {
                return Results.NotFound("Author not found :/");
            }

            await authorService.UpdateAuthorAsync(author);
            return Results.Ok("Author updated");
        });

        app.MapDelete("/authors/delete/{id:guid}",
            async (Guid id, IAuthorReadRepository authorReadRepository, IAuthorRepository authorRepository) =>
            {
                var author = await authorReadRepository.GetAuthorByIdAsync(id);// przerobić na serwis z warstwy application
                if (author == null)
                {
                    return Results.NotFound("Author not found");
                }

                authorRepository.DeleteAuthor(author); // przerobić na serwis z warstwy application
                return Results.Ok("Author deleted");
            });

        app.MapGet("/authors/{id:guid}", async (Guid id, IAuthorReadRepository authorReadRepository) =>
        {
            var author = await authorReadRepository.GetAuthorByIdAsync(id);// przerobić na serwis z warstwy application
            return author != null ? Results.Ok(author) : Results.NotFound("Author not found");
        });
        
        app.MapGet("/authors/getAuthors", async (IAuthorService authorService) =>
            await authorService.GetAuthorsDictionaryAsync() is { } author
                ? Results.Ok(author)
                : Results.NotFound());
    }
}