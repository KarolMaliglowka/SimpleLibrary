using Library.Core.Entities;
using Library.Core.Repositories;

namespace Library.Api.EndPoints;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this WebApplication app)
    {
        app.MapGet("/author", async (IAuthorReadRepository authorReadRepository) =>
        {
            var authors = await authorReadRepository.GetAuthorsAsync();
            return Results.Ok(authors);
        });

        app.MapPost("/author/create", async (Author author, IAuthorRepository authorRepository) =>
        {
            await authorRepository.AddAuthorAsync(author);
            return Results.Created($"/author/{author.Id}", author);
        });

        app.MapPost("/author/createMany", async (List<Author> authors, IAuthorRepository authorRepository) =>
        {
            await authorRepository.AddAuthorsAsync(authors);
            return Results.Created();
        });

        app.MapPut("/author/update", async (
            Author author,
            IAuthorReadRepository authorReadRepository,
            IAuthorRepository authorRepository
        ) =>
        {
            var authorInDb = await authorReadRepository.GetAuthorByIdAsync(author.Id);
            if (authorInDb == null)
            {
                return Results.NotFound("Author not found :/");
            }

            await authorRepository.UpdateAuthorAsync(author);
            return Results.Ok("Author updated");
        });

        app.MapDelete("/author/delete/{id:guid}",
            async (Guid id, IAuthorReadRepository authorReadRepository, IAuthorRepository authorRepository) =>
            {
                var author = await authorReadRepository.GetAuthorByIdAsync(id);
                if (author == null)
                {
                    return Results.NotFound("Author not found");
                }

                await authorRepository.DeleteAuthorAsync(author);
                return Results.Ok("Author deleted");
            });

        app.MapGet("/author/{id:guid}", async (Guid id, IAuthorReadRepository authorReadRepository) =>
        {
            var author = await authorReadRepository.GetAuthorByIdAsync(id);
            return author != null ? Results.Ok(author) : Results.NotFound("Author not found....");
        });
    }
}