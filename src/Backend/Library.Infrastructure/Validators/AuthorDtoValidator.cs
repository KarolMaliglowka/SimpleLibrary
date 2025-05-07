using FluentValidation;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Validators;

public class AuthorDtoValidator : AbstractValidator<AuthorDto>
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorDtoValidator(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;

        RuleFor(a => a.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Author name cannot be empty");

        RuleFor(x => x)
            .MustAsync((model, _) => NotExists(model.Name, model.Surname))
            .WithMessage("Author with this name already exists.");
    }

    private async Task<bool> NotExists(string authorName, string? authorSurname)
    {
        return await _authorRepository
            .ExistAuthorAsync(authorName, authorSurname);
    }
}