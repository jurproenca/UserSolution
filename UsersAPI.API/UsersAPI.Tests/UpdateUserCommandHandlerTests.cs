using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using OpenQA.Selenium;

public class UpdateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_UpdatesUser()
    {
        // ARRANGE
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Criar um usuário inicial no banco de dados
        using (var arrangeContext = new ApplicationDbContext(options))
        {
            arrangeContext.Users.Add(new Users
            {
                Id = 1,
                Name = "Old Name",
                Email = "old@email.com",
                UpdatedAt = DateTime.MinValue
            });
            await arrangeContext.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var handler = new UpdateUserCommandHandler(context);
            var command = new UpdateUserCommand
            {
                Id = 1,
                Name = "New Name",
                Email = "new@email.com"
            };

            // ACT
            await handler.Handle(command, CancellationToken.None);

            // ASSERT
            var updatedUser = await context.Users.FindAsync(1);
            updatedUser.Should().NotBeNull();
            updatedUser.Name.Should().Be("New Name");
            updatedUser.Email.Should().Be("new@email.com");
            updatedUser.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
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
            var handler = new UpdateUserCommandHandler(context);
            var command = new UpdateUserCommand
            {
                Id = 999, // ID que não existe
                Name = "New Name",
                Email = "new@email.com"
            };

            // ACT & ASSERT
            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }

    [Fact]
    public async Task Handle_UpdateOnlyName_KeepsEmailUnchanged()
    {
        // ARRANGE
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Criar um usuário inicial no banco de dados
        using (var arrangeContext = new ApplicationDbContext(options))
        {
            arrangeContext.Users.Add(new Users
            {
                Id = 1,
                Name = "Original Name",
                Email = "original@email.com"
            });
            await arrangeContext.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var handler = new UpdateUserCommandHandler(context);
            var command = new UpdateUserCommand
            {
                Id = 1,
                Name = "Updated Name",
                Email = null
            };

            // ACT
            await handler.Handle(command, CancellationToken.None);

            // ASSERT
            var updatedUser = await context.Users.FindAsync(1);
            updatedUser.Should().NotBeNull();
            updatedUser.Name.Should().Be("Updated Name");
        }
    }
}