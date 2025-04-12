using Library.Core.ValueObjects;

namespace Library.Infrastructure.DTO;

public record BaseDto
{
    public string Name { get; set; }
}