namespace Library.Core.Entities;

public class Author : BaseClass
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public ICollection<Book> Books { get; set; }
    public string FullName => $"{Name} {Surname}";
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Author(string? name, string? surname = null)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetSurname(surname);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
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