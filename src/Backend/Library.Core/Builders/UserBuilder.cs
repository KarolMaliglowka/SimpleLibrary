using System.Text.RegularExpressions;
using Library.Core.Entities;
using Library.Core.ValueObjects;

namespace Library.Core.Builders;

public sealed class UserBuilder
{
    private readonly User _user;

    public UserBuilder()
    {
        _user = new User
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public UserBuilder(User existingUser)
    {
        _user = existingUser;
    }

    public UserBuilder SetName(Name name)
    {
        ValidateInput(name.Value, "Name", 2);
        _user.Name = name;
        return this;
    }

    public UserBuilder SetSurname(string surname)
    {
        ValidateInput(surname, "Surname", 2);
        _user.Surname = surname;
        return this;
    }

    public UserBuilder SetEmail(string email)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(email))
        {
            throw new ArgumentException("Invalid email format.");
        }

        _user.Email = email;
        return this;
    }

    public UserBuilder SetAddress(string address)
    {
        ValidateInput(address, "Address", 2);
        _user.Address = address;
        return this;
    }

    public UserBuilder SetPhoneNumber(string phoneNumber)
    {
        ValidateInput(phoneNumber, "PhoneNumber", 9);
        _user.PhoneNumber = phoneNumber;
        return this;
    }

    public UserBuilder SetCity(string city)
    {
        ValidateInput(city, "City", 2);
        _user.City = city;
        return this;
    }

    public UserBuilder SetCountry(string country)
    {
        ValidateInput(country, "Country", 2);
        _user.Country = country;
        return this;
    }

    public UserBuilder SetPostalCode(string postalCode)
    {
        ValidateInput(postalCode, "PostalCode", 5);
        _user.PostalCode = postalCode;
        return this;
    }

    public UserBuilder SetActive(bool isActive)
    {
        _user.IsActive = isActive;
        return this;
    }

    public User Build()
    {
        _user.UpdatedAt = DateTime.UtcNow;
        return _user;
    }

    private static void ValidateInput(string input, string fieldName, int minLength = 1)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < minLength)
        {
            throw new ArgumentException(
                $"{fieldName} cannot be empty and must have at least {minLength} characters.");
        }
    }
}