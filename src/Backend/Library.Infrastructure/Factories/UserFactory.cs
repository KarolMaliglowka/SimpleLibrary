using Library.Core.Entities;
using Library.Core.ValueObjects;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Factories;

public static class UserFactory
{
    // public static User CreateUser(UserDto userDto)
    // {
    //     ArgumentNullException.ThrowIfNull(userDto);
    //
    //     return new User.Builder()
    //         .SetName(new Name(userDto.Name))
    //         .SetSurname(userDto.Surname)
    //         .SetEmail(userDto.Email)
    //         .SetAddress(userDto.Address)
    //         .SetPhoneNumber(userDto.PhoneNumber)
    //         .SetCity(userDto.City)
    //         .SetCountry(userDto.Country)
    //         .SetPostalCode(userDto.PostalCode)
    //         .SetActive(true)
    //         .Build();
    // }
    //
    // public static User EditUser(UserDto userDto, User currentUser)
    // {
    //     ArgumentNullException.ThrowIfNull(userDto);
    //
    //     return new User.Builder(currentUser)
    //         .SetName(new Name(userDto.Name))
    //         .SetSurname(userDto.Surname)
    //         .SetEmail(userDto.Email)
    //         .SetAddress(userDto.Address)
    //         .SetPhoneNumber(userDto.PhoneNumber)
    //         .SetCity(userDto.City)
    //         .SetCountry(userDto.Country)
    //         .SetPostalCode(userDto.PostalCode)
    //         .SetActive(userDto.IsActive)
    //         .Build();
    // }

    public static User ActiveUser(bool isActive, User currentUser)
    {
        return new User.Builder(currentUser)
            .SetActive(isActive)
            .Build();
    }

    public static User BuildUser(UserDto userDto, User? currentUser = null)
    {
        ArgumentNullException.ThrowIfNull(userDto);
        return currentUser switch
        {
            null => new User.Builder()
                .SetName(new Name(userDto.Name))
                .SetSurname(userDto.Surname)
                .SetEmail(userDto.Email)
                .SetAddress(userDto.Address)
                .SetPhoneNumber(userDto.PhoneNumber)
                .SetCity(userDto.City)
                .SetCountry(userDto.Country)
                .SetPostalCode(userDto.PostalCode)
                .SetActive(true)
                .Build(),
            _ => new User.Builder(currentUser)
                .SetName(new Name(userDto.Name))
                .SetSurname(userDto.Surname)
                .SetEmail(userDto.Email)
                .SetAddress(userDto.Address)
                .SetPhoneNumber(userDto.PhoneNumber)
                .SetCity(userDto.City)
                .SetCountry(userDto.Country)
                .SetPostalCode(userDto.PostalCode)
                .SetActive(userDto.IsActive)
                .Build()
        };
    }
}