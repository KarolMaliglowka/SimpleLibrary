using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly LibraryDbContext _context;
    
    public PublisherRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Publisher>> GetPublishersAsync() => 
        await _context.Publishers
        .AsNoTracking()
        .ToListAsync();

    public async Task<Publisher> AddPublisherAsync(Publisher publisher)
    {
        await _context.Publishers.AddAsync(publisher);
        await _context.SaveChangesAsync();
        return publisher;
    }

    public async Task<Publisher?> GetPublisherByIdAsync(Guid id) => 
        await _context.Publishers
            .SingleOrDefaultAsync(p => p.Id == id);
    
    public async Task<Publisher?> GetPublisherByNameAsync(string name) => 
        await _context.Publishers
            .SingleOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    
    public async Task<bool> ExistAuthorAsync(Publisher publisher) =>
        await _context.Publishers
            .AnyAsync(p => p.Name == publisher.Name);
    
    public async Task AddPublishersAsync(List<Publisher> publishers)
    {
        await _context.Publishers.AddRangeAsync(publishers);
        await _context.SaveChangesAsync();
    }
}