using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using OpenQA.Selenium;

public class DeleteUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidUserId_DeletesUser()
    {
        // ARRANGE
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Criar um usuário inicial no banco de dados
        using (var arrangeContext = new ApplicationDbContext(options))
        {
            arrangeContext.Users.Add(new Users { Id = 1, Name = "Test", Email = "test@test.com" });
            await arrangeContext.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var handler = new DeleteUserCommandHandler(context);
            var command = new DeleteUserCommand { Id = 1 };

            // ACT
            await handler.Handle(command, CancellationToken.None);

            // ASSERT
            var user = await context.Users.FindAsync(1);
            user.Should().BeNull();
        }
    }

    [Fact]
    public async Task Handle_InvalidUserId_ThrowsNotFoundException()
    {
        // ARRANGE
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var handler = new DeleteUserCommandHandler(context);
            var command = new DeleteUserCommand { Id = 999 }; // ID que não existe

            // ACT & ASSERT
            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}