using System.Text.RegularExpressions;
using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public sealed class User : BaseClass
{
    public User()
    {
    }

    public User(Name name, string surname, string email, string address, string phoneNumber, string city,
        string country, string postalCode)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetSurname(surname);
        Email = email;
        Address = address;
        PhoneNumber = phoneNumber;
        City = city;
        Country = country;
        PostalCode = postalCode;
        CreatedAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public string UserIdentity { get; set; }
    public Name Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public List<Borrow> Borrows { get; set; } = [];
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public string FullName => $"{Name.Value} {Surname}";
    

    public void SetName(Name name)
    {
        ValidateInput(name, "Name", 2);
        Name = name;
        UpdateTimestamp();
    }

    public void SetSurname(string surname)
    {
        ValidateInput(surname, "Surname", 2);
        Surname = surname;
        UpdateTimestamp();
    }
    
    public void SetEmail(string email)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(email))
        {
            throw new ArgumentException("Invalid email format.");
        }
        Email = email;
        UpdateTimestamp();
    }
    public void SetAddress(string address)
    {
        ValidateInput(address, "Address", 2);
        Address = address;
        UpdateTimestamp();
    }
    public void SetPhoneNumber(string phoneNumber)
    {
        ValidateInput(phoneNumber, "PhoneNumber", 9);
        PhoneNumber = phoneNumber;
        UpdateTimestamp();
    }
    public void SetCity(string city)
    {
        ValidateInput(city, "City", 2);
        City = city;
        UpdateTimestamp();
    }
    public void SetCountry(string country)
    {
        ValidateInput(country, "Country", 2);
        Country = country;
        UpdateTimestamp();
    }
    public void SetPostalCode(string postalCode)
    {
        ValidateInput(postalCode, "PostalCode", 5);
        PostalCode = postalCode;
        UpdateTimestamp();
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
    
    private static void ValidateInput(string input, string fieldName, int minLength = 1)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < minLength)
        {
            throw new ArgumentException($"{fieldName} cannot be empty and must have at least {minLength} characters.");
        }
    }
}