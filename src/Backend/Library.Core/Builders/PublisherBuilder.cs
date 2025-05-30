using Library.Core.Entities;

namespace Library.Core.Builders;

public sealed class PublisherBuilder
{
    private readonly Publisher _publisher;

    public PublisherBuilder()
    {
        _publisher = new Publisher
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public PublisherBuilder(Publisher publisher)
    {
        _publisher = publisher;
    }

    public PublisherBuilder SetName(string name)
    {
        ValidateInput(name, "Publisher", 2);
        _publisher.Name = name;
        return this;
    }

    public Publisher Build()
    {
        _publisher.UpdatedAt = DateTime.UtcNow;
        return _publisher;
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