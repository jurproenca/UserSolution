using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

public class GetAllUsersQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public GetAllUsersQueryHandlerTests()
    {
        // Configurar o AutoMapper
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Users, UserDto>();
        });
        _mapper = configuration.CreateMapper();

        // Configurar o banco de dados em memória
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetAllUsersTestDb")
            .Options;
    }

    [Fact]
    public async Task Handle_WithUsersInDatabase_ReturnsListOfUserDtos()
    {
        // ARRANGE
        using (var arrangeContext = new ApplicationDbContext(_options))
        {
            arrangeContext.Users.AddRange(
                new Users { Id = 1, Name = "User 1", Email = "user1@test.com" },
                new Users { Id = 2, Name = "User 2", Email = "user2@test.com" }
            );
            await arrangeContext.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_options))
        {
            var handler = new GetAllUsersQueryHandler(context, _mapper);

            // ACT
            var result = await handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.Should().Contain(u => u.Id == 1 && u.Name == "User 1" && u.Email == "user1@test.com");
            result.Should().Contain(u => u.Id == 2 && u.Name == "User 2" && u.Email == "user2@test.com");
        }
    }

    [Fact]
    public async Task Handle_EmptyDatabase_ReturnsEmptyList()
    {
        // ARRANGE - Banco de dados vazio
        using (var context = new ApplicationDbContext(_options))
        {
            // Garantir que o banco está vazio
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();

            var handler = new GetAllUsersQueryHandler(context, _mapper);

            // ACT
            var result = await handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task Handle_MapsAllPropertiesCorrectly()
    {
        // ARRANGE
        using (var arrangeContext = new ApplicationDbContext(_options))
        {
            arrangeContext.Users.Add(new Users
            {
                Id = 3,
                Name = "Test User",
                Email = "test@user.com",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            });
            await arrangeContext.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_options))
        {
            var handler = new GetAllUsersQueryHandler(context, _mapper);

            // ACT
            var result = await handler.Handle(new GetAllUsersQuery(), CancellationToken.None);
            var user = result.FirstOrDefault(u => u.Id == 3);

            // ASSERT
            user.Should().NotBeNull();
            user.Name.Should().Be("Test User");
            user.Email.Should().Be("test@user.com");
            // Adicione mais asserts para outras propriedades se necessário
        }
    }
}