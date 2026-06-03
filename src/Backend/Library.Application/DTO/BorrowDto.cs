namespace Library.Application.DTO;

public class BorrowDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string? BookName { get; set; }
    public List<AuthorDto>? BookAuthors { get; set; }
    public Guid UserId { get; set; }
    public string? UserFullName { get; set; }
    public DateTime BorrowDate { get; set; }
}