using System.Text.RegularExpressions;
using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public sealed class User : BaseClass
{
    public Name Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string PostalCode { get; private set; }
    public List<Borrow> Borrows { get; private set; } = [];
    public DateTime? CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public string FullName => $"{Name.Value} {Surname}";

    private User()
    {
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public class Builder
    {
        private readonly User user;

        public Builder()
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
        }

        public Builder(User existingUser)
        {
            user = existingUser;
        }

        public Builder SetName(Name name)
        {
            ValidateInput(name.Value, "Name", 2);
            user.Name = name;
            return this;
        }

        public Builder SetSurname(string surname)
        {
            ValidateInput(surname, "Surname", 2);
            user.Surname = surname;
            return this;
        }

        public Builder SetEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
            {
                throw new ArgumentException("Invalid email format.");
            }

            user.Email = email;
            return this;
        }

        public Builder SetAddress(string address)
        {
            ValidateInput(address, "Address", 2);
            user.Address = address;
            return this;
        }

        public Builder SetPhoneNumber(string phoneNumber)
        {
            ValidateInput(phoneNumber, "PhoneNumber", 9);
            user.PhoneNumber = phoneNumber;
            return this;
        }

        public Builder SetCity(string city)
        {
            ValidateInput(city, "City", 2);
            user.City = city;
            return this;
        }

        public Builder SetCountry(string country)
        {
            ValidateInput(country, "Country", 2);
            user.Country = country;
            return this;
        }

        public Builder SetPostalCode(string postalCode)
        {
            ValidateInput(postalCode, "PostalCode", 5);
            user.PostalCode = postalCode;
            return this;
        }

        public Builder SetActive(bool isActive)
        {
            user.IsActive = isActive;
            return this;
        }

        public User Build()
        {
            user.UpdateTimestamp();
            return user;
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
}