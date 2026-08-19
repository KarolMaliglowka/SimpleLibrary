using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class AuthorRepository : IAuthorRepository, IAuthorReadRepository
{
    public AuthorRepository()
    {
    }

    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAuthorsWithBooksAsync() => await _context.Authors
        .Include(a => a.Books)
        .ToListAsync();

    public async Task<List<Author>> GetAuthorsAsync() => await _context.Authors
        .OrderBy(x => x.Name)
        .ToListAsync();

    public Task<List<Author>> GetAuthorBySurnameAsync(string surname) =>
        _context.Authors.Where(a =>
            a.Surname.ToLower() == surname.ToLower()
        ).ToListAsync();

    public Task<Author?> GetAuthorsBySurnameAndNameAsync(string? surName, string? name = null)
    {
        return surName != null ? _context.Authors
            .FirstOrDefaultAsync(a => a.Name == name &&  a.Surname == surName) : null;
    }
    

    public async Task<Author?> GetAuthorByIdAsync(Guid? id) =>
        await _context.Authors
            .Where(p => !p.IsDeleted)
            .SingleOrDefaultAsync(a => a.Id == id);

    public async Task AddAuthorAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
    }

    public async Task UpdateAuthorAsync(Author author)
    {
        var existingAuthor = await GetAuthorByIdAsync(author.Id);
        if (existingAuthor == null)
        {
            throw new Exception("Author not found");
        }

        _context.Authors.Update(author);
    }

    public async Task<bool> ExistAuthorAsync(Author author)
    {
        return await _context.Authors
            .AsNoTracking()
            .AnyAsync(a => a.Name == author.Name && a.Surname == author.Surname);
    }

    public async Task DeleteAuthor(Author author)
    {
        var existingAuthor = await GetAuthorByIdAsync(author.Id);
        if (existingAuthor == null)
        {
            throw new Exception("Author not found");
        }

        _context.Authors.Remove(author);
    }

    public void AddAuthors(List<Author> authors)
    {
        _context.Authors.AddRange(authors);
    }

    public Task<Author?> GetAuthorAsync(string? surname, string? name = null) =>
        _context.Authors.SingleOrDefaultAsync(a =>
            a.Surname == surname &&
            (name == null || (a.Name != null && a.Name == name))
        );

    public Task<bool> ExistAuthorAsync(string name, string? surname = null) =>
        _context.Authors
            .AsNoTracking()
            .AnyAsync(x =>
                x.Name == name &&
                x.Surname == surname
            );

    public async Task<List<Author>> GetAuthorsListBySurnameAndName(string? surName, string? name = null) => 
     await _context.Authors
         .Where(x => x.Surname == surName && name != null && x.Name == name)
         .ToListAsync();
}