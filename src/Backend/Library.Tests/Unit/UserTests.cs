using Library.Core.Builders;

namespace Library.Tests.Unit;

public class UserTests
{
    private const string Name = "John";
    private const string Surname = "Doe";
    
    [Fact]
    public void Build_ShouldCreateUserWithDefaultValues()
    {
        // Act
        var user = new UserBuilder().Build();

        // Assert
        Assert.NotNull(user);
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.NotNull(user.CreatedAt);
    }
    
    [Fact]
    public void SetName_ShouldSetUserName()
    {
        // Arrange
        var userBuilder = new UserBuilder();

        // Act
        userBuilder.SetName(Name);
        var user = userBuilder.Build();

        // Assert
        Assert.Equal(Name, user.Name);
    }
    
    [Fact]
    public void SetSurname_ShouldSetUserSurname()
    {
        // Arrange
        var userBuilder = new UserBuilder();

        // Act
        userBuilder.SetSurname(Surname);
        var user = userBuilder.Build();

        // Assert
        Assert.Equal(Surname, user.Surname);
    }
}