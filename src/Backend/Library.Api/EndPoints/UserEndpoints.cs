using Library.Infrastructure.DTO;
using Library.Infrastructure.Services;

namespace Library.Api.EndPoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/user/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var authors = await userService.GetUserById(id);
            return Results.Ok(authors);
        });
        
        app.MapGet("/user/{surname}", async (string surname, IUserService userService) =>
        {
            var author = await userService.GetUserBySurname(surname);
            return Results.Ok(author);
        });
        
        app.MapGet("/user", async (IUserService userService) =>
        {
            var authors = await userService.GetUsers();
            return Results.Ok(authors);
        });
        
        app.MapPost("/user/create", async (UserDto userDto, IUserService userService) =>
        {
            await userService.CreateUserAsync(userDto);
            return Results.Created();
        });
        
        app.MapPost("/user/createMany", async (List<UserDto> usersDto, IUserService userService) =>
        {
            await userService.CreateUsersAsync(usersDto);
            return Results.Created();
        });
        
        app.MapPatch("/user/update", async (UserDto userDto, IUserService userService) =>
        {
            await userService.UpdateUser(userDto);
            return Results.Ok();
        });
        
        app.MapPatch("/user/activate", async (UserDto userDto, IUserService userService) =>
        {
            await userService.SetUserActive(userDto.Id, userDto.IsActive);
            return Results.Ok();
        });
    }
}