using Library.Core.Builders;
using Library.Core.Entities;

namespace Library.Tests.Unit;

public class PublisherBuilderTests
{
    private const string Name = "Custom Publisher";

    [Fact]
    public void Build_ShouldCreatePublisherWithDefaultValues()
    {
        // Act
        var publisher = new PublisherBuilder().Build();

        // Assert
        Assert.NotNull(publisher);
        Assert.NotEqual(Guid.Empty, publisher.Id);
        Assert.NotNull(publisher.CreatedAt);
    }

    [Fact]
    public void SetName_ShouldSetPublisherName()
    {
        // Arrange
        var publisherBuilder = new PublisherBuilder();

        // Act
        publisherBuilder.SetName(Name);
        var publisher = publisherBuilder.Build();

        // Assert
        Assert.Equal(Name, publisher.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("E")]
    public void SetPublisher_ShouldThrowException(string? name)
    {
        // Arrange
        var publisherBuilder = new PublisherBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => publisherBuilder.SetName(name));
    }

    [Fact]
    public void PublisherBuilder_ShouldInitializeFromExistingPublisher()
    {
        // Arrange
        var existingPublisher = new Publisher
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Name = Name
        };

        // Act
        var newPublisher = new PublisherBuilder(existingPublisher)
            .Build();

        // Assert
        Assert.Equal(existingPublisher.Id, newPublisher.Id);
        Assert.Equal(existingPublisher.Name, newPublisher.Name);
        Assert.Equal(existingPublisher.CreatedAt, newPublisher.CreatedAt);
    }
}