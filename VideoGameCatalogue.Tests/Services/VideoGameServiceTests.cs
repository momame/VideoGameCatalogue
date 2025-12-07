using Moq;
using FluentAssertions;
using VideoGameCatalogue.Core.Entities;
using VideoGameCatalogue.Core.Interfaces;
using VideoGameCatalogue.Core.Services;
using VideoGameCatalogue.Core.DTOs;
using Xunit;

namespace VideoGameCatalogue.Tests.Services;

/// <summary>
/// Unit tests for VideoGameService.
/// Uses Moq to mock repository - tests service logic in isolation.
/// </summary>
public class VideoGameServiceTests
{
    private readonly Mock<IVideoGameRepository> _repositoryMock;
    private readonly VideoGameService _service;

    public VideoGameServiceTests()
    {
        _repositoryMock = new Mock<IVideoGameRepository>();
        _service = new VideoGameService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllGamesAsDtos()
    {
        // Arrange
        var games = new List<VideoGame>
        {
            new VideoGame { Id = 1, Title = "Game 1", Genre = "Action" },
            new VideoGame { Id = 2, Title = "Game 2", Genre = "RPG" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(games);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<VideoGameDto>();
        result.First().Title.Should().Be("Game 1");

        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnDto()
    {
        // Arrange
        var game = new VideoGame
        {
            Id = 1,
            Title = "Test Game",
            Genre = "Action",
            Rating = 8.5m
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(game);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Test Game");
        result.Genre.Should().Be("Action");
        result.Rating.Should().Be(8.5m);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((VideoGame?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldMapDtoToEntityAndReturnDto()
    {
        // Arrange
        var createDto = new CreateVideoGameDto
        {
            Title = "New Game",
            Genre = "Sports",
            Rating = 7.5m,
            Price = 49.99m
        };

        var createdGame = new VideoGame
        {
            Id = 10,
            Title = createDto.Title,
            Genre = createDto.Genre,
            Rating = createDto.Rating,
            Price = createDto.Price
        };

        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<VideoGame>()))
            .ReturnsAsync(createdGame);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.Title.Should().Be("New Game");
        result.Genre.Should().Be("Sports");

        _repositoryMock.Verify(r => r.CreateAsync(
            It.Is<VideoGame>(g => g.Title == "New Game")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var existingGame = new VideoGame
        {
            Id = 1,
            Title = "Original Title",
            Genre = "Action"
        };

        var updateDto = new UpdateVideoGameDto
        {
            Title = "Updated Title",
            Genre = "RPG",
            Rating = 9.0m
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existingGame);

        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<VideoGame>()))
            .ReturnsAsync((VideoGame game) => game);

        // Act
        var result = await _service.UpdateAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Updated Title");
        result.Genre.Should().Be("RPG");
        result.Rating.Should().Be(9.0m);

        _repositoryMock.Verify(r => r.UpdateAsync(
            It.Is<VideoGame>(g => g.Title == "Updated Title")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var updateDto = new UpdateVideoGameDto { Title = "Test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((VideoGame?)null);

        // Act
        var result = await _service.UpdateAsync(999, updateDto);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<VideoGame>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
    }
}