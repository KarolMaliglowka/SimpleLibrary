using Library.Core.Entities;

namespace Library.Core.Repositories;

public interface IPublisherRepository
{
    Task<List<Publisher>> GetPublishersAsync();
    Task<Publisher> AddPublisherAsync(Publisher publisher);
    Task<Publisher?> GetPublisherByIdAsync(Guid id);
    Task<Publisher?> GetPublisherByNameAsync(string name);
    Task<bool> ExistAuthorAsync(Publisher publisher);
    Task AddPublishersAsync(List<Publisher> publishers);
}