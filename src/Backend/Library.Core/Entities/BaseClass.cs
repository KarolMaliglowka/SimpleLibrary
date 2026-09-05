namespace Library.Core.Entities;

public abstract class BaseClass
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}