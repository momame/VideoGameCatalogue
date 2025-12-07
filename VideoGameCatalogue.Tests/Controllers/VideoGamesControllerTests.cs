using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VideoGameCatalogue.API.Controllers;
using VideoGameCatalogue.Core.DTOs;
using VideoGameCatalogue.Core.Interfaces;
using Xunit;

namespace VideoGameCatalogue.Tests.Controllers;

/// <summary>
/// Unit tests for VideoGamesController.
/// Tests HTTP layer logic - status codes, error handling, routing.
/// </summary>
public class VideoGamesControllerTests
{
    private readonly Mock<IVideoGameService> _serviceMock;
    private readonly Mock<ILogger<VideoGamesController>> _loggerMock;
    private readonly VideoGamesController _controller;

    public VideoGamesControllerTests()
    {
        _serviceMock = new Mock<IVideoGameService>();
        _loggerMock = new Mock<ILogger<VideoGamesController>>();
        _controller = new VideoGamesController(_serviceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturn200WithGames()
    {
        // Arrange
        var games = new List<VideoGameDto>
        {
            new VideoGameDto { Id = 1, Title = "Game 1" },
            new VideoGameDto { Id = 2, Title = "Game 2" }
        };

        _serviceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(games);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);

        var returnedGames = okResult.Value.Should().BeAssignableTo<IEnumerable<VideoGameDto>>().Subject;
        returnedGames.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturn200WithGame()
    {
        // Arrange
        var game = new VideoGameDto { Id = 1, Title = "Test Game" };

        _serviceMock.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(game);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);

        var returnedGame = okResult.Value.Should().BeOfType<VideoGameDto>().Subject;
        returnedGame.Title.Should().Be("Test Game");
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturn404()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(999))
            .ReturnsAsync((VideoGameDto?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_WithValidDto_ShouldReturn201()
    {
        // Arrange
        var createDto = new CreateVideoGameDto { Title = "New Game" };
        var createdGame = new VideoGameDto { Id = 10, Title = "New Game" };

        _serviceMock.Setup(s => s.CreateAsync(createDto))
            .ReturnsAsync(createdGame);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.ActionName.Should().Be(nameof(_controller.GetById));

        var returnedGame = createdResult.Value.Should().BeOfType<VideoGameDto>().Subject;
        returnedGame.Id.Should().Be(10);
    }

    [Fact]
    public async Task Update_WithValidId_ShouldReturn200()
    {
        // Arrange
        var updateDto = new UpdateVideoGameDto { Title = "Updated Title" };
        var updatedGame = new VideoGameDto { Id = 1, Title = "Updated Title" };

        _serviceMock.Setup(s => s.UpdateAsync(1, updateDto))
            .ReturnsAsync(updatedGame);

        // Act
        var result = await _controller.Update(1, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedGame = okResult.Value.Should().BeOfType<VideoGameDto>().Subject;
        returnedGame.Title.Should().Be("Updated Title");
    }

    [Fact]
    public async Task Update_WithInvalidId_ShouldReturn404()
    {
        // Arrange
        var updateDto = new UpdateVideoGameDto { Title = "Test" };

        _serviceMock.Setup(s => s.UpdateAsync(999, updateDto))
            .ReturnsAsync((VideoGameDto?)null);

        // Act
        var result = await _controller.Update(999, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturn204()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_WithInvalidId_ShouldReturn404()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}