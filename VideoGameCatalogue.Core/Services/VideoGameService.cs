using VideoGameCatalogue.Core.DTOs;
using VideoGameCatalogue.Core.Entities;
using VideoGameCatalogue.Core.Interfaces;

namespace VideoGameCatalogue.Core.Services;

/// <summary>
/// Service layer implementing business logic for video game operations.
/// Sits between controllers (HTTP layer) and repository (data layer).
/// Follows SRP - handles business logic and DTO/Entity mapping only, no HTTP or database concerns.
/// Implements IVideoGameService interface for DIP - consumers depend on abstraction.
/// </summary>
public class VideoGameService : IVideoGameService
{
    private readonly IVideoGameRepository _repository;

    /// <summary>
    /// Constructor with repository interface injection.
    /// Using interface (not concrete class) follows DIP - service doesn't know about EF Core or SQL.
    /// Makes service testable - can inject mock repository for unit tests.
    /// </summary>
    public VideoGameService(IVideoGameRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retrieves all video games and maps to DTOs for API consumption.
    /// Service handles Entity → DTO transformation, keeping API contracts separate from database structure.
    /// If database schema changes, only this mapping changes - API contract stays stable (OCP).
    /// </summary>
    public async Task<IEnumerable<VideoGameDto>> GetAllAsync()
    {
        // Get entities from repository
        var games = await _repository.GetAllAsync();

        // Map each entity to DTO using centralized mapping method
        // Using LINQ Select for clean, functional transformation
        return games.Select(MapToDto);
    }

    /// <summary>
    /// Retrieves a specific video game by ID and returns as DTO.
    /// Returns null if not found - controller will translate this to HTTP 404.
    /// Service doesn't know about HTTP status codes - maintains separation of concerns.
    /// </summary>
    public async Task<VideoGameDto?> GetByIdAsync(int id)
    {
        var game = await _repository.GetByIdAsync(id);

        // Return null if not found, otherwise map to DTO
        // Null propagation - clean and readable
        return game != null ? MapToDto(game) : null;
    }

    /// <summary>
    /// Creates a new video game from CreateDTO.
    /// Service orchestrates: DTO → Entity → Repository → Entity → DTO
    /// This is where we'd add business rules (e.g., duplicate title check, price validation).
    /// </summary>
    public async Task<VideoGameDto> CreateAsync(CreateVideoGameDto createDto)
    {
        // Map DTO to Entity - transformation happens in service layer
        // Not using AutoMapper to keep dependencies minimal and logic explicit
        var game = new VideoGame
        {
            Title = createDto.Title,
            Genre = createDto.Genre,
            ReleaseDate = createDto.ReleaseDate,
            Publisher = createDto.Publisher,
            Rating = createDto.Rating,
            Price = createDto.Price,
            Description = createDto.Description
            // Note: Id, CreatedAt, UpdatedAt are set by repository/database
        };

        // Delegate persistence to repository - service doesn't know about DbContext
        var created = await _repository.CreateAsync(game);

        // Map result back to DTO for API response
        return MapToDto(created);
    }

    /// <summary>
    /// Updates an existing video game.
    /// Service checks existence first - returns null if not found (allows controller to return 404).
    /// Preserves Id and CreatedAt from existing entity - only updates provided fields.
    /// </summary>
    public async Task<VideoGameDto?> UpdateAsync(int id, UpdateVideoGameDto updateDto)
    {
        // First verify the game exists - don't assume repository will throw
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return null;  // Not found - controller handles 404

        // Update entity properties from DTO
        // Keeping original Id and CreatedAt intact - only updating modifiable fields
        existing.Title = updateDto.Title;
        existing.Genre = updateDto.Genre;
        existing.ReleaseDate = updateDto.ReleaseDate;
        existing.Publisher = updateDto.Publisher;
        existing.Rating = updateDto.Rating;
        existing.Price = updateDto.Price;
        existing.Description = updateDto.Description;
        // UpdatedAt will be set by repository

        // Persist changes via repository
        var updated = await _repository.UpdateAsync(existing);

        // Return updated entity as DTO
        return MapToDto(updated);
    }

    /// <summary>
    /// Deletes a video game by ID.
    /// Delegates to repository - returns success/failure for controller to handle.
    /// Could add business rules here (e.g., check if game has reviews before deleting).
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        // Repository returns false if not found, true if deleted
        return await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps VideoGame entity to VideoGameDto.
    /// Centralized mapping ensures consistency - single source of truth for transformation.
    /// Private method as it's internal implementation detail.
    /// In larger apps, would use AutoMapper, but explicit mapping aids code clarity here.
    /// </summary>
    /// <param name="game">The entity to map</param>
    /// <returns>Mapped DTO</returns>
    private static VideoGameDto MapToDto(VideoGame game)
    {
        return new VideoGameDto
        {
            Id = game.Id,
            Title = game.Title,
            Genre = game.Genre,
            ReleaseDate = game.ReleaseDate,
            Publisher = game.Publisher,
            Rating = game.Rating,
            Price = game.Price,
            Description = game.Description
            // Note: Not exposing CreatedAt/UpdatedAt to API consumers
            // These are internal audit fields, not part of API contract
        };
    }
}