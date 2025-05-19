using Library.Core.Entities;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Factories;

public static class PublisherFactory
{

    public static Publisher CreatePublisher(PublisherDto publisherDto)
    {
        ArgumentNullException.ThrowIfNull(publisherDto);
        return new Publisher.Builder()
            .SetName(publisherDto.Name)
            .Build();
    }
    
    public static Publisher EditPublisher(PublisherDto publisherDto, Publisher publisher)
    {
        ArgumentNullException.ThrowIfNull(publisherDto);
        return new Publisher.Builder(publisher)
            .SetName(publisherDto.Name)
            .Build();
    }
}