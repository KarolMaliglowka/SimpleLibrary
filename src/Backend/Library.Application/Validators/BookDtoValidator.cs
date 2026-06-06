using FluentValidation;
using Library.Application.DTO;
using Library.Core.Repositories;

namespace Library.Application.Validators;

public class BookDtoValidator : AbstractValidator<BookDto>
{
    private readonly IBookRepository _bookRepository;

    public BookDtoValidator(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;

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
        var guidsList = authors.Select(x => x.Id!.Value).ToList();
        return !await _bookRepository.ExistsAsync(bookName, guidsList , bookId);
    }
}