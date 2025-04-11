namespace Library.Infrastructure.DTO;

public class BorrowDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    public DateTime BorrowDate { get; set; }
}