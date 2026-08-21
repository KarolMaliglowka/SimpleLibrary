using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public sealed class Category : BaseClass
{
    
    private readonly List<Book> _books = [];
    public Category()
    {
    }
    
    public Category(Name name)
    {
        SetCategory(name);
        IsDeleted = false;
    }
    
    public Name Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public ICollection<Book> Books => _books.AsReadOnly();

    public void SetCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category) || category.Length < 3)
        {
            throw new ArgumentException("Category cannot be empty. It requires minimum 4 characters.");
        }

        Name = category;
    }
    
    public void SetSoftDelete()
    {
        IsDeleted = true;
    }
}