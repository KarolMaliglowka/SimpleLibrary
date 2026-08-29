using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public class Author : BaseClass
{
    public Name? Name { get; private set; }
    public string? Surname { get; private set; }
    public ICollection<Book> Books { get; set; }
    public string FullName => $"{Name} {Surname}";
    public bool IsDeleted { get; private set; }

    public Author(string? name, string? surname = null)
    {
        SetName(name!);
        SetSurname(surname!);
        IsDeleted = false;
    }

    public Author()
    {
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
        {
            throw new ArgumentException("Name cannot be empty. It requires minimum 3 characters.");
        }

        Name = name;
    }

    public void SetSurname(string surname)
    {
        Surname = surname;
    }
    
    public void SetSoftDelete()
    {
        IsDeleted = true;
    }
}

