using Library.Core.ValueObjects;

namespace Library.Infrastructure.DTO;

public record BookDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<AuthorDto>? Authors { get; set; }
    public int PagesCount { get; set; }
    public string? Description { get; set; }
    public PublisherDto? Publisher { get; set; }
    public string? Isbn { get; set; }
    public string? YearOfRelease { get; set; }
    public CategoryDto? Category { get; set; }
    public bool IsBorrowed { get; set; }
}