using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Services;

public interface IUserService
{
    Task CreateUserAsync(UserDto userDto);
    Task UpdateUser(UserDto userDto);
    Task<UserDto> GetUserById(Guid id);
    Task<UserDto> GetUserByName(string name);
    Task<UserDto> GetUserBySurname(string surname);
    Task<List<UserDto>> GetUsers();
    Task SetUserActive(Guid userId, bool isActive);
    Task CreateUsersAsync(List<UserDto> usersDto);
    Task<UserDto> GetUserWithBorrowedBooksById(Guid id);
    Task<List<UserDto>> GetUsersWithBorrowedBooks();
}

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<UserDto>> GetUsers()
    {
        var users = await userRepository.GetUsersAsync();
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            City = user.City,
            Country = user.Country,
            PostalCode = user.PostalCode,
            IsActive = user.IsActive,
            FullName = user.FullName
        }).ToList();
    }

    public async Task CreateUserAsync(UserDto userDto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Surname = userDto.Surname,
            Email = userDto.Email,
            Address = userDto.Address,
            PhoneNumber = userDto.PhoneNumber,
            City = userDto.City,
            Country = userDto.Country,
            PostalCode = userDto.PostalCode,
        };
        user.UserIdentity = GenerateCode(10);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        user.SetActive(true);

        await userRepository.AddUserAsync(user);
    }

    public async Task UpdateUser(UserDto userDto)
    {
        var user = await userRepository.GetUserByIdAsync(userDto.Id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.SetName(userDto.Name);
        user.SetSurname(userDto.Surname);
        user.SetEmail(userDto.Email);
        user.SetAddress(userDto.Address);
        user.SetPhoneNumber(userDto.PhoneNumber);
        user.SetCity(userDto.City);
        user.SetCountry(userDto.Country);
        user.SetPostalCode(userDto.PostalCode);

        await userRepository.UpdateUser(user);
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            City = user.City,
            Country = user.Country,
            PostalCode = user.PostalCode,
            IsActive = user.IsActive,
            FullName = user.FullName
        };
    }

    public async Task<UserDto> GetUserByName(string name)
    {
        var user = await userRepository.GetUserByNameAsync(name);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            City = user.City,
            Country = user.Country,
            PostalCode = user.PostalCode,
            IsActive = user.IsActive,
            FullName = user.FullName
        };
    }

    public async Task<UserDto> GetUserBySurname(string surname)
    {
        var user = await userRepository.GetUserBySurnameAsync(surname);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            City = user.City,
            Country = user.Country,
            PostalCode = user.PostalCode,
            IsActive = user.IsActive,
            FullName = user.FullName
        };
    }

    public async Task SetUserActive(Guid userId, bool isActive)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.SetActive(isActive);

        await userRepository.UpdateUser(user);
    }

    public async Task CreateUsersAsync(List<UserDto> usersDto)
    {
        var usersList = usersDto.Distinct().ToList();
        var usersExistInSystem = await userRepository.GetUsersAsync();
        var usersToImport = usersList
            .Where(x => usersExistInSystem.All(y => y.Name.ToLower() != x.Name.ToLower()));

        var users = usersToImport.Select(user => new User()
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                Country = user.Country,
                PostalCode = user.PostalCode,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserIdentity = GenerateCode(10)
            })
            .ToList();

        await userRepository.AddUsersAsync(users);
    }

    public async Task<UserDto> GetUserWithBorrowedBooksById(Guid id)
    {
        var user = await userRepository.GetUserWithBorrowedBooksByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            City = user.City,
            Country = user.Country,
            PostalCode = user.PostalCode,
            IsActive = user.IsActive,
            FullName = user.FullName,
            Books = user.Borrows.Select(x => x.Book).ToList().Select(y => new BookDto()
            {
                Id = y.Id,
                Name = y.Name,
                PagesCount = y.PagesCount,
                Description = y.Description,
                Publisher = new PublisherDto { Name = y.Publisher?.Name, Id = y.Publisher.Id },
                Isbn = y.ISBN,
                YearOfRelease = y.YearOfRelease,
                Category = new CategoryDto { Name = y.Category?.Name, Id = y.Category.Id},
                Authors = y.Authors?.Select(s => new AuthorDto
                    {
                        Name = s.Name ?? "",
                        Surname = s.Surname ?? "",
                        Id = s.Id
                    })
                    .ToList(),
                IsBorrowed = y.IsBorrowed
            }).ToList()
        };
    }

    public async Task<List<UserDto>> GetUsersWithBorrowedBooks()
    {
        var users = await userRepository.GetUsersWithBorrowedBooksAsync();
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            City = user.City,
            Country = user.Country,
            PostalCode = user.PostalCode,
            IsActive = user.IsActive,
            FullName = user.FullName,
            Books = user.Borrows.Select(x => x.Book).ToList().Select(y => new BookDto()
            {
                Id = y.Id,
                Name = y.Name,
                PagesCount = y.PagesCount,
                Description = y.Description,
                Publisher = new PublisherDto { Name = y.Publisher?.Name, Id = y.Publisher.Id },
                Isbn = y.ISBN,
                YearOfRelease = y.YearOfRelease,
                Category = new CategoryDto { Name = y.Category?.Name, Id = y.Category.Id},
                Authors = y.Authors?.Select(s => new AuthorDto
                    {
                        Name = s.Name ?? "",
                        Surname = s.Surname ?? "",
                        Id = s.Id
                    })
                    .ToList(),
                IsBorrowed = y.IsBorrowed
            }).ToList()
        }).Where(x => x.Books.Any()).ToList();
    }

    private static string GenerateCode(int longString)
    {
        const string znaki = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(znaki, longString)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }
}