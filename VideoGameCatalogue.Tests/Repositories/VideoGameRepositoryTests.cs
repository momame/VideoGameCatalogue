using Microsoft.EntityFrameworkCore;
using VideoGameCatalogue.Core.Entities;
using VideoGameCatalogue.Infrastructure.Data;
using VideoGameCatalogue.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;

namespace VideoGameCatalogue.Tests.Repositories;

/// <summary>
/// Unit tests for VideoGameRepository.
/// Uses in-memory database to test data access logic without hitting real SQL Server.
/// </summary>
public class VideoGameRepositoryTests : IDisposable
{
    private readonly VideoGameDbContext _context;
    private readonly VideoGameRepository _repository;

    public VideoGameRepositoryTests()
    {
        // Create in-memory database with unique name for test isolation
        var options = new DbContextOptionsBuilder<VideoGameDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new VideoGameDbContext(options);
        _repository = new VideoGameRepository(_context);

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var games = new List<VideoGame>
        {
            new VideoGame
            {
                Id = 1,
                Title = "Test Game 1",
                Genre = "Action",
                Publisher = "Test Publisher",
                Rating = 8.5m,
                Price = 59.99m,
                CreatedAt = DateTime.UtcNow
            },
            new VideoGame
            {
                Id = 2,
                Title = "Test Game 2",
                Genre = "RPG",
                Publisher = "Test Publisher 2",
                Rating = 9.0m,
                Price = 49.99m,
                CreatedAt = DateTime.UtcNow
            }
        };

        _context.VideoGames.AddRange(games);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllGames()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(g => g.Title == "Test Game 1");
        result.Should().Contain(g => g.Title == "Test Game 2");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnGame()
    {
        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Game 1");
        result.Genre.Should().Be("Action");
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddGameToDatabase()
    {
        // Arrange
        var newGame = new VideoGame
        {
            Title = "New Test Game",
            Genre = "Sports",
            Rating = 7.5m,
            Price = 39.99m
        };

        // Act
        var result = await _repository.CreateAsync(newGame);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        var allGames = await _repository.GetAllAsync();
        allGames.Should().HaveCount(3);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingGame()
    {
        // Arrange
        var game = await _repository.GetByIdAsync(1);
        game!.Title = "Updated Title";
        game.Rating = 9.5m;

        // Act
        var result = await _repository.UpdateAsync(game);

        // Assert
        result.Title.Should().Be("Updated Title");
        result.Rating.Should().Be(9.5m);
        result.UpdatedAt.Should().NotBeNull();
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldRemoveGameAndReturnTrue()
    {
        // Act
        var result = await _repository.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();

        var allGames = await _repository.GetAllAsync();
        allGames.Should().HaveCount(1);
        allGames.Should().NotContain(g => g.Id == 1);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();

        var allGames = await _repository.GetAllAsync();
        allGames.Should().HaveCount(2); // No games deleted
    }

    [Fact]
    public async Task ExistsAsync_WithValidId_ShouldReturnTrue()
    {
        // Act
        var result = await _repository.ExistsAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}