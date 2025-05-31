using Library.Core.Builders;
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
            return new PublisherBuilder()
                .SetName(publisherDto.Name)
                .Build();
        }

        return new PublisherBuilder(publisher)
            .SetName(publisherDto.Name)
            .Build();
    }
}