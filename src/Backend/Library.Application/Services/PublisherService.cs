using Library.Application.DTO;
using Library.Core;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

public interface IPublisherService
{
    Task<List<PublisherDto>> GetPublishersAsync();
    Task CreatePublisherAsync(PublisherDto publisher);
    Task UpdatePublisher(PublisherDto publisher);
    Task<PublisherDto> GetPublisherByIdAsync(Guid id);
    Task<PublisherDto> GetPublisherByNameAsync(string name);
    Task<List<Dictionary<Guid, string>>> GetPublishersDictionaryAsync();
    Task DeletePublisherAsync(Guid guid);
}

public class PublisherService(IPublisherRepository publisherRepository, IUnitOfWork unitOfWork, IBookRepository bookRepository, ILogger<PublisherService> logger) : IPublisherService
{
    public async Task<List<PublisherDto>> GetPublishersAsync()
    {
        var publishersList = await publisherRepository.GetPublishersAsync();
        return publishersList.Select(p => new PublisherDto()
            {
                Id = p.Id,
                Name = p.Name,
                isDelete = p.IsDeleted
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
            logger.Log(LogLevel.Error, "{PublisherName} '{Name}' already exists.", nameof(Publisher), publisher.Name);
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
            logger.Log(LogLevel.Error, "Publisher with id: '{PublisherName}' not found", publisher.Name);
            throw new PublisherNotFoundException(publisher.Name);
        }

        existingPublisher.SetPublisher(publisher.Name);
        
        publisherRepository.UpdatePublisher(existingPublisher);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<PublisherDto> GetPublisherByIdAsync(Guid id)
    {
        var publisher = await publisherRepository.GetPublisherByIdAsync(id);
        if (publisher != null)
            return new PublisherDto()
            {
                Id = publisher.Id,
                Name = publisher.Name
            };
        logger.Log(LogLevel.Error, "Publisher with id: '{PublisherName}' not found", publisher.Name);
        throw new PublisherNotFoundException($"with id: { id }");
    }

    public async Task<PublisherDto> GetPublisherByNameAsync(string name)
    {
        var publisher = await publisherRepository.GetPublisherByNameAsync(name);
        if (publisher != null)
            return new PublisherDto()
            {
                Id = publisher.Id,
                Name = publisher.Name
            };
        logger.Log(LogLevel.Error, "Publisher with id: '{PublisherName}' not found", publisher.Name);
        throw new  PublisherNotFoundException(name);
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
    
    public async Task DeletePublisherAsync(Guid id)
    {
        var publisherExist = await publisherRepository.GetPublisherByIdAsync(id);
        if (publisherExist == null)
        {
            logger.Log(LogLevel.Error, "Publisher with id: '{PublisherId}' not found", id);
            throw new PublisherNotFoundException($"{id}");
        }

        var booksList = await bookRepository.GetAllAsync();
        var isPublisherForSomeBook = booksList.Any(x => (x.Publisher?.Name.Value.ToLower()).Equals(publisherExist.Name.Value, StringComparison.CurrentCultureIgnoreCase));
        if (isPublisherForSomeBook)
        {
            throw new PublisherIsInUseException(publisherExist.Name);
        }

        publisherExist.SetSoftDelete();
        publisherRepository.UpdatePublisher(publisherExist);
        await unitOfWork.SaveChangesAsync();
    }
}