using Library.Core.Entities;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Factories;

public static class PublisherFactory
{
    public static Publisher CreatePublisher(PublisherDto publisherDto, Publisher? publisher = null)
    {
        ArgumentNullException.ThrowIfNull(publisherDto);
        if (publisher == null)
        {
            return new Publisher.Builder()
                .SetName(publisherDto.Name)
                .Build();
        }

        return new Publisher.Builder(publisher)
            .SetName(publisherDto.Name)
            .Build();
    }
}