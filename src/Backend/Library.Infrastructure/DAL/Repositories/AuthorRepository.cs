using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class AuthorRepository : IAuthorRepository
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
        .Include(a => a.Books).ToListAsync();

    public async Task<List<Author>> GetAuthorsAsync() => await _context.Authors
        .AsNoTracking()
        .ToListAsync();

    public Task<List<Author>> GetAuthorBySurnameAsync(string surname) =>
        _context.Authors.Where(a =>
            a.Surname.ToLower() == surname.ToLower()
        ).ToListAsync();

    public Task<Author?> GetAuthorByNameAsync(string name) =>
        _context.Authors.SingleOrDefaultAsync(a => a.Name == name);

    public Task<Author?> GetAuthorByIdAsync(Guid id) =>
        _context.Authors.SingleOrDefaultAsync(a => a.Id == id);

    public async Task AddAuthorAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAuthorAsync(Author author)
    {
        var existingAuthor = await GetAuthorByIdAsync(author.Id);
        if (existingAuthor == null)
        {
            throw new Exception("Author not found");
        }

        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistAuthorAsync(Author author)
    {
        return await _context.Authors
            .AsNoTracking()
            .AnyAsync(a => a.Name == author.Name && a.Surname == author.Surname);
    }

    public async Task DeleteAuthorAsync(Author author)
    {
        var existingAuthor = await GetAuthorByIdAsync(author.Id);
        if (existingAuthor == null)
        {
            throw new Exception("Author not found");
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }

    public async Task AddAuthorsAsync(List<Author> authors)
    {
        _context.Authors.AddRange(authors);
        await _context.SaveChangesAsync();
    }

    public Task<Author?> GetAuthorAsync(string surname, string? name = null) =>
        _context.Authors.SingleOrDefaultAsync(a =>
            a.Surname.ToLower() == surname.ToLower() &&
            a.Name.Value.ToLower() == name.ToLower()
        );
}