namespace Library.Infrastructure.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException(string? name = null)
        : base($"Category {name} not found")
    {
    }
}