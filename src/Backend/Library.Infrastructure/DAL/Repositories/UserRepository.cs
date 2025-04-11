using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class UserRepository(LibraryDbContext context) : IUserRepository
{
    public async Task<List<User>> GetUsersAsync() =>
        await context.Users
            .AsNoTracking()
            .ToListAsync();

    public async Task<User?> GetUserByIdAsync(Guid id) =>
        await context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetUserByNameAsync(string name) =>
        await context.Users.FirstOrDefaultAsync(u =>
            u.Name.ToLower() == name.ToLower());

    public async Task<User?> GetUserBySurnameAsync(string surname) =>
        await context.Users.FirstOrDefaultAsync(u =>
            u.Surname.ToLower() == surname.ToLower());

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task AddUsersAsync(List<User> users)
    {
        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}