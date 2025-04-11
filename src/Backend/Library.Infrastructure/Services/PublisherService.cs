using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Services;

public interface IPublisherService
{
    Task<List<PublisherDto>> GetPublishersAsync();
    Task CreatePublisherAsync(PublisherDto publisher);
    Task UpdatePublisher(PublisherDto publisher);
    Task<PublisherDto> GetPublisherByIdAsync(Guid id);
    Task<PublisherDto> GetPublisherByNameAsync(string name);
}

public class PublisherService(IPublisherRepository publisherRepository) : IPublisherService
{
    public async Task<List<PublisherDto>> GetPublishersAsync()
    {
        var publishersList = await publisherRepository.GetPublishersAsync();
        return publishersList.Select(p => new PublisherDto()
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
    }

    public async Task CreatePublisherAsync(PublisherDto publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);

        var newPublisher = new Publisher(publisher.Name);
        await publisherRepository.AddPublisherAsync(newPublisher);
    }

    public async Task UpdatePublisher(PublisherDto publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);
        var oldPublisher = await publisherRepository.GetPublisherByIdAsync(publisher.Id);
        if (oldPublisher == null)
        {
            throw new NullReferenceException("Publisher not found");
        }

        oldPublisher.SetPublisher(publisher.Name);
    }

    public async Task<PublisherDto> GetPublisherByIdAsync(Guid id)
    {
        var publisher = await publisherRepository.GetPublisherByIdAsync(id);
        if (publisher == null)
        {
            throw new NullReferenceException("Publisher not found");
        }

        return new PublisherDto()
        {
            Id = publisher.Id,
            Name = publisher.Name
        };
    }

    public async Task<PublisherDto> GetPublisherByNameAsync(string name)
    {
        var publisher = await publisherRepository.GetPublisherByNameAsync(name);
        if (publisher == null)
        {
            throw new NullReferenceException("Publisher not found");
        }

        return new PublisherDto()
        {
            Id = publisher.Id,
            Name = publisher.Name
        };
    }
}