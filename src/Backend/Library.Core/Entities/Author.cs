using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public class Author : BaseClass
{
    public Name? Name { get; private set; }
    public string? Surname { get; private set; }
    public ICollection<Book> Books { get; set; }
    public string FullName => $"{Name} {Surname}";

    public Author(string? name, string? surname = null)
    {
        Id = Guid.NewGuid();
        SetName(name!);
        SetSurname(surname!);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Author()
    {
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 1)
        {
            throw new ArgumentException("Name cannot be empty. It requires minimum 3 characters.");
        }

        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetSurname(string surname)
    {
        Surname = surname;
        UpdatedAt = DateTime.UtcNow;
    }
}