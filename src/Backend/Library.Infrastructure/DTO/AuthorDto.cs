using Library.Core.ValueObjects;

namespace Library.Infrastructure.DTO;

public record AuthorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}