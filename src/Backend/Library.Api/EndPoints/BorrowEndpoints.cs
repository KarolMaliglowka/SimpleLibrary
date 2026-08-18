using Library.Application.DTO;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.EndPoints;

public static class BorrowEndpoints
{
    public static void MapBorrowEndpoints(this WebApplication app)
    {
        app.MapPost("/borrows/create", async ([FromBody]BorrowRequestDto borrowDto, IBorrowService borrowService ) =>
        {
            await borrowService.CreateBorrow(borrowDto);
            return Results.Created();
        });
        
        app.MapDelete("/borrows/delete/{id:guid}", async (Guid id, IBorrowService borrowService ) =>
        {
            await borrowService.DeleteBorrow(id);
            return Results.Ok();
        });

        app.MapGet("/borrows", async (IBookService bookService) =>
        {
            var borrowingBooks = await bookService.GetBorrowingBooksWithUsers();
            return borrowingBooks.Count == 0 ? Results.NotFound("No books found.") : Results.Ok(borrowingBooks);
        });
    }
}