using FluentValidation;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Validoators;

public class BookDtoValidator : AbstractValidator<BookDto>
{
    private readonly IBookRepository _bookService;

    public BookDtoValidator(IBookRepository bookService)
    {
        _bookService = bookService;

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

        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x)
                .MustAsync((model, _) => NotExists(model.Name, model.Id, model.Authors))
                .OverridePropertyName(nameof(BookDto.Name))
                .WithMessage("Book with this name and authors already exists.");
        });
    }

    private async Task<bool> NotExists(string bookName, Guid bookId, List<AuthorDto> authors)
    {
        return !await _bookService
            .QueryAsNoTracking()
            .AnyAsync(x =>
                x.Id != bookId &&
                x.Name == bookName 
               );
    }
}