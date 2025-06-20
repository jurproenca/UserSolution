using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsUserId()
    {
        // ARRANGE
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nome único
            .Options;

        using var context = new ApplicationDbContext(options);
        var handler = new CreateUserCommandHandler(context);

        // ACT
        var result = await handler.Handle(
            new CreateUserCommand { Name = "Test", Email = "test@test.com" },
            CancellationToken.None
        );

        // ASSERT
        result.Should().BeGreaterThan(0);
        context.Users.Should().Contain(u => u.Id == result);
    }
}