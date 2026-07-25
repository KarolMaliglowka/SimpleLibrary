namespace Library.Application.DTO;

public record PublisherDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public bool IsDelete { get; set; }
}