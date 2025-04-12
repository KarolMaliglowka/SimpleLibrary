using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public class User : BaseClass
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
    public List<Borrow> Borrows { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public string FullName => $"{Surname} {Name}";
    

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            throw new ArgumentException("Name cannot be empty. It requires minimum 3 characters.");
        }

        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetSurname(string surname)
    {
        if (surname.Length < 2)
        {
            throw new ArgumentException("Surname cannot be less than 3 characters.");
        }

        Surname = surname;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            throw new ArgumentException("Email cannot be empty and must contain '@'.");
        }

        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }
    public void SetAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address cannot be empty.");
        }

        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }
    public void SetPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number cannot be empty.");
        }

        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }
    public void SetCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City cannot be empty.");
        }

        City = city;
        UpdatedAt = DateTime.UtcNow;
    }
    public void SetCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country cannot be empty.");
        }

        Country = country;
        UpdatedAt = DateTime.UtcNow;
    }
    public void SetPostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            throw new ArgumentException("Postal code cannot be empty.");
        }

        PostalCode = postalCode;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }
}