using Library.Core.Entities;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure;

public static class Extensions
{
    public static UserDto BuildUserDto(this User user)
    {
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
}