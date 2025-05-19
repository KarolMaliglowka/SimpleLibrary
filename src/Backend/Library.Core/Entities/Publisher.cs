using Library.Core.ValueObjects;

namespace Library.Core.Entities;
public sealed class Publisher : BaseClass
{
    private readonly List<Book> _books = [];

    public Name Name { get; set; }
    public IEnumerable<Book> Books => _books.AsReadOnly();
    
    private void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    
    public class Builder
    {
        private readonly Publisher _publisher;

        public Builder()
        {
            _publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
        }
        
        public Builder(Publisher publisher)
        {
            _publisher = publisher;
        }

        public Builder SetName(string name)
        {
            ValidateInput(name, "Publisher", 2);
            _publisher.Name = name;
            return this;
        }
        
        public Publisher Build()
        {
            _publisher.UpdateTimestamp();
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
}