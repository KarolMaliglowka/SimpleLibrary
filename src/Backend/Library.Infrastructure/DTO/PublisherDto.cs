namespace Library.Infrastructure.DTO;

public record PublisherDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}