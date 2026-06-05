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
        var publisher = new Publisher();

        // Assert
        Assert.NotNull(publisher);
        Assert.NotEqual(Guid.Empty, publisher.Id);
        Assert.NotNull(publisher.CreatedAt);
    }

    [Fact]
    public void SetName_ShouldSetPublisherName()
    {
        // Arrange
        var publisher = new Publisher();

        // Act
        publisher.SetPublisher(Name);

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
        var publisher = new Publisher();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => publisher.SetPublisher(name!));
    }

    [Fact]
    public void PublisherBuilder_ShouldInitializeFromExistingPublisher()
    {
        // Arrange
        var existingPublisher = new Publisher(Name);

        // Act

        // Assert
        Assert.Equal(existingPublisher.Id, existingPublisher.Id);
        Assert.Equal(existingPublisher.Name, existingPublisher.Name);
        Assert.Equal(existingPublisher.CreatedAt, existingPublisher.CreatedAt);
    }
}