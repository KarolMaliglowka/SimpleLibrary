using Library.Core.Repositories;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Factories;

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
        var newPublisher = PublisherFactory.Publisher(publisher); 
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
        var publisherToUpdate = PublisherFactory.Publisher(publisher,oldPublisher); 
        await publisherRepository.UpdatePublisherAsync(publisherToUpdate);
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