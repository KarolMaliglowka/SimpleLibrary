using Library.Infrastructure.DTO;
using Library.Infrastructure.Services;

namespace Library.Api.EndPoints;

public static class PublisherEndpoints
{
    public static void MapPublisherEndpoints(this WebApplication app)
    {
        app.MapGet("/publisher", async (IPublisherService publisherService) =>
            await publisherService.GetPublishersAsync() is { } publishers
                ? Results.Ok(publishers)
                : Results.NotFound("No publishers found"));

        app.MapPost("/publisher/create", async (PublisherDto publisherDto, IPublisherService publisherService) =>
        {
            await publisherService.CreatePublisherAsync(publisherDto);
            return Results.Created();
        });

        app.MapPatch("/publisher/update", async (PublisherDto publisherDto, IPublisherService publisherService) =>
        {
            await publisherService.UpdatePublisher(publisherDto);
            return Results.Ok();
        });
        
        app.MapGet("/publisher/{id:guid}", async (Guid id, IPublisherService publisherService) =>
        {
            var publisher = await publisherService.GetPublisherByIdAsync(id);
            return Results.Ok(publisher);
        });
        
        app.MapGet("/publisher/{name}", async (string name, IPublisherService publisherService) =>
        {
            var publisher = await publisherService.GetPublisherByNameAsync(name);
            return Results.Ok(publisher);
        });
    }
}