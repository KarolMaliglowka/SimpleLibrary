namespace Library.Application.DTO;

public record CategoryDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
}