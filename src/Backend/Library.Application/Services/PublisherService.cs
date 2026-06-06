using Library.Application.DTO;
using Library.Application.Factories;
using Library.Core;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;

namespace Library.Application.Services;

public interface IPublisherService
{
    Task<List<PublisherDto>> GetPublishersAsync();
    Task CreatePublisherAsync(PublisherDto publisher);
    Task UpdatePublisher(PublisherDto publisher);
    Task<PublisherDto> GetPublisherByIdAsync(Guid id);
    Task<PublisherDto> GetPublisherByNameAsync(string name);
    Task<List<Dictionary<Guid, string>>> GetPublishersDictionaryAsync();
}

public class PublisherService(IPublisherRepository publisherRepository, IUnitOfWork unitOfWork) : IPublisherService
{
    public async Task<List<PublisherDto>> GetPublishersAsync()
    {
        var publishersList = await publisherRepository.GetPublishersAsync();
        return publishersList.Select(p => new PublisherDto()
            {
                Id = p.Id,
                Name = p.Name
            })
            .OrderBy(x => x.Name)
            .ToList();
    }

    public async Task CreatePublisherAsync(PublisherDto publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);
        var existingPublisher = await publisherRepository.GetPublisherByNameAsync(publisher.Name);
        if (existingPublisher != null)
        {
            throw new AlreadyExistsException(nameof(Publisher), publisher.Name);
        }
        var newPublisher = new Publisher(publisher.Name);
        await publisherRepository.AddPublisherAsync(newPublisher);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdatePublisher(PublisherDto publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);
        var existingPublisher = await publisherRepository.GetPublisherByIdAsync(publisher.Id);
        if (existingPublisher == null)
        {
            throw new PublisherNotFoundException(publisher.Name);
        }

        existingPublisher.SetPublisher(publisher.Name);
        
        publisherRepository.UpdatePublisher(existingPublisher);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<PublisherDto> GetPublisherByIdAsync(Guid id)
    {
        var publisher = await publisherRepository.GetPublisherByIdAsync(id);
        if (publisher == null)
        {
            throw new PublisherNotFoundException($"with id: { id }");
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
            throw new  PublisherNotFoundException(name);
        }

        return new PublisherDto()
        {
            Id = publisher.Id,
            Name = publisher.Name
        };
    }
    
    public async Task<List<Dictionary<Guid, string>>> GetPublishersDictionaryAsync()
    {
        var publishersList = await publisherRepository.GetPublishersAsync();
        return publishersList
            .OrderBy(x => x.Name.Value)
            .Select(x => new Dictionary<Guid, string>
            {
                [x.Id] = x.Name.Value
            })
            .ToList();
    }
}