namespace Library.Infrastructure.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException()
        : base("Category not found")
    {
    }
}