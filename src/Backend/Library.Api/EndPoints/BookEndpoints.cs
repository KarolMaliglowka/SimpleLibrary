using FluentValidation;
using Library.Api.Extensions;
using Library.Application.DTO;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.EndPoints;

public static class BookEndpoints
{
    public static void MapBookEndpoint(this WebApplication app)
    {
        app.MapGet("/books", async (IBookService bookService) =>
        {
            var books = await bookService.GetAllBooksAsync();
            return books.Count == 0 ? Results.NotFound("No books found.") : Results.Ok(books);
            //do zmiany dto
        });

        app.MapPost("/books/create",
            async (BookDto book,
                IBookService bookService
                //[FromServices] IValidator<BookDto> bookValidator,
                //HttpContext context
                ) =>
            {
               // var validateResult = await bookValidator.ValidateCommandAsync(book, context);
                //if (validateResult != Results.Empty) return validateResult;
                await bookService.CreateBookAsync(book);
                return Results.Created();
            });

        app.MapPost("/books/createMany",
            async (List<BookDto> books, IBookService bookService) =>
            {
                await bookService.CreateBooksAsync(books);
                return Results.Created();
            });

        app.MapGet("/books/{id:guid}", async (Guid id, IBookService bookService) =>
        {
            var book = await bookService.GetBookByIdAsync(id);
            return Results.Ok(book);
        });

        app.MapPatch("/books/update",
            async (BookDto book,
                IBookService bookService,
                [FromServices] IValidator<BookDto> bookValidator,
                HttpContext context) =>
            {
                var validateResult = await bookValidator.ValidateCommandAsync(book, context);
                if (validateResult != Results.Empty) return validateResult;
                await bookService.UpdateBook(book);
                return Results.Ok();
            });

        app.MapGet("/books/author",
            async ([FromQuery] string surname, [FromQuery] string name, IBookService bookService) =>
            {
                var book = await bookService.GetBooksByAuthorAsync(surname, name);
                return Results.Ok(book);
            });

        app.MapGet("/books/category", async ([FromQuery] string name, IBookService bookService) =>
        {
            var book = await bookService.GetBooksByCategoryAsync(name);
            return Results.Ok(book);
        });

        app.MapGet("/books/publisher", async ([FromQuery] string name, IBookService bookService) =>
        {
            var book = await bookService.GetBooksByPublisherAsync(name);
            return Results.Ok(book);
        });

        app.MapGet("/books/{name}", async (string name, IBookService bookService) =>
            await bookService.GetBookByNameAsync(name)
                is { } book
                ? Results.Ok(book)
                : Results.NotFound("Book not found"));
        
        
        app.MapGet("/books/getbooks", async (IBookService bookService) =>
        {
            var books = await bookService.GetBooksDictionaryAsync();
            return books.Count == 0 ? Results.NotFound("No books found.") : Results.Ok(books);
            //do zmiany dto
        });
    }
}