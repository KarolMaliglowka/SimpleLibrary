using Library.Application.DTO;
using Library.Application.Services;

namespace Library.Api.EndPoints;

public static class PublisherEndpoints
{
    public static void MapPublisherEndpoints(this WebApplication app)
    {
        app.MapGet("/publishers", async (IPublisherService publisherService) =>
            await publisherService.GetPublishersAsync() is { } publishers
                ? Results.Ok(publishers)
                : Results.NotFound("No publishers found"));

        app.MapPost("/publishers/create", async (PublisherDto publisherDto, IPublisherService publisherService) =>
        {
            await publisherService.CreatePublisherAsync(publisherDto);
            return Results.Created();
        });

        app.MapPatch("/publishers/update", async (PublisherDto publisherDto, IPublisherService publisherService) =>
        {
            await publisherService.UpdatePublisher(publisherDto);
            return Results.Ok();
        });
        
        app.MapGet("/publishers/{id:guid}", async (Guid id, IPublisherService publisherService) =>
        {
            var publisher = await publisherService.GetPublisherByIdAsync(id);
            return Results.Ok(publisher);
        });
        
        app.MapGet("/publishers/{name}", async (string name, IPublisherService publisherService) =>
        {
            var publisher = await publisherService.GetPublisherByNameAsync(name);
            return Results.Ok(publisher);
        });
        
        app.MapGet("/publishers/getPublishers", async (IPublisherService publisherService) =>
            await publisherService.GetPublishersDictionaryAsync() is { } publishers
                ? Results.Ok(publishers)
                : Results.NotFound("No publishers found"));
    }
}