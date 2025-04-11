namespace Library.Infrastructure.DTO;

public record CategoryDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}