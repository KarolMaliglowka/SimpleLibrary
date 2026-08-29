using Library.Application.DTO;
using Library.Application.Services;

namespace Library.Api.EndPoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var authors = await userService.GetUserById(id);
            return Results.Ok(authors);
        });

        app.MapGet("/users/{surname}", async (string surname, IUserService userService) =>
        {
            var author = await userService.GetUserBySurname(surname);
            return Results.Ok(author);
        });

        app.MapGet("/users", async (IUserService userService) =>
        {
            var authors = await userService.GetUsers();
            return Results.Ok(authors);
        });
        
        app.MapGet("/users/withbooks/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var authors = await userService.GetUserWithBorrowedBooksById(id);
            return Results.Ok(authors);
        });

        app.MapGet("/users/withbooks", async (IUserService userService) =>
        {
            var authors = await userService.GetUsersWithBorrowedBooks();
            return Results.Ok(authors);
        });
        
        app.MapGet("/users/getUsers", async (IUserService userService) =>
            await userService.GetUsersDictionaryAsync() is { } users
                ? Results.Ok(users)
                : Results.NotFound("No publishers found"));
        
        app.MapPost("/users", async (UserDto userDto, IUserService userService) =>
        {
            await userService.CreateUserAsync(userDto);
            return Results.Created();
        });

        app.MapPost("/users/createMany", async (List<UserDto> usersDto, IUserService userService) =>
        {
            await userService.CreateUsersAsync(usersDto);
            return Results.Created();
        });

        app.MapPatch("/users", async (UserDto userDto, IUserService userService) =>
        {
            await userService.UpdateUser(userDto);
            return Results.Ok();
        });

        app.MapPatch("/users/activate/{id:guid}", async (Guid id, IUserService userService) =>
        {
            await userService.SetUserActive(id, true);
            return Results.Ok();
        });

        app.MapPatch("/users/deactivate/{id:guid}", async (Guid id, IUserService userService) =>
        {
            await userService.SetUserActive(id, false);
            return Results.Ok();
        });
        
        app.MapDelete("/users/{id:guid}", async (Guid id, IUserService userService) =>
            await userService.DeleteUserAsync(id));
    }
}