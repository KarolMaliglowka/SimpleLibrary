namespace Library.Application.DTO;

public record AuthorDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsDelete { get; set; }
}