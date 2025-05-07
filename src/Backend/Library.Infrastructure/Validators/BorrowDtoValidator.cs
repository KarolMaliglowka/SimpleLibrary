using FluentValidation;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Validators;

public class BorrowDtoValidator : AbstractValidator<BorrowDto>
{
    private readonly IBorrowRepository _borrowRepository;

    public BorrowDtoValidator(IBorrowRepository borrowRepository)
    {
        _borrowRepository = borrowRepository;

        RuleFor(b => b.BookId)
            .NotEmpty()
            .NotNull()
            .WithMessage("BookId is required");

        RuleFor(b => b.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId is required");

        RuleFor(b => b.BorrowDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("BorrowDate is required");

        RuleFor(x => x)
            .MustAsync((model, _) => NotExists(model.BookId))
            .WithMessage("The book with this Id is already borrowed.");
    }

    private async Task<bool> NotExists(Guid bookId)
    {
        return await _borrowRepository
            .ExistBorrowAsync(bookId);
    }
}