namespace Library.Core.Exceptions;

public class CategoryNotFoundException : CustomException
{
    public string Name { get; }

    public CategoryNotFoundException(string name) : base($"Category '{name}' not found.")
    {
        Name = name;
    }
}