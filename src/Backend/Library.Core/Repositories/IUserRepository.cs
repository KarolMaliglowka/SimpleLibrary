using Library.Core.Entities;

namespace Library.Core.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByNameAsync(string name);
    Task<User?> GetUserBySurnameAsync(string surname);
    Task AddUserAsync(User user);
    Task UpdateUser(User user);
    Task AddUsersAsync(List<User> users);
}