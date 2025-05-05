using FluentValidation;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Validoators;

public class BookDtoValidator : AbstractValidator<BookDto>
{
    public BookDtoValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name is required");
        RuleFor(b => b.Authors)
            .NotEmpty()
            .NotNull()
            .WithMessage("Authors are required");
        RuleFor(b => b.Category)
            .NotEmpty()
            .NotNull()
            .WithMessage("Category is required");
    }
}