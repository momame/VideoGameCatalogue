using VideoGameCatalogue.Core.DTOs;

namespace VideoGameCatalogue.Core.Interfaces;

/// <summary>
/// Service interface defining business operations for video game catalogue.
/// Service layer sits between controllers and repositories - handles business logic and DTO/Entity mapping.
/// Follows SRP - controllers handle HTTP, services handle business logic, repositories handle data access.
/// </summary>
public interface IVideoGameService
{
    /// <summary>
    /// Retrieves all video games as DTOs for API consumption.
    /// Service handles entity-to-DTO mapping, shielding API from internal domain changes.
    /// </summary>
    Task<IEnumerable<VideoGameDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific video game by ID as DTO.
    /// Returns null if not found - controller will translate to 404 response.
    /// </summary>
    /// <param name="id">The unique identifier of the video game</param>
    /// <returns>VideoGameDto if found, null otherwise</returns>
    Task<VideoGameDto?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new video game from a create DTO.
    /// Service maps DTO to entity, delegates persistence to repository, maps result back to DTO.
    /// </summary>
    /// <param name="createDto">The video game data from API request</param>
    /// <returns>The created video game as DTO with generated ID</returns>
    Task<VideoGameDto> CreateAsync(CreateVideoGameDto createDto);

    /// <summary>
    /// Updates an existing video game.
    /// Service handles existence check, mapping, and delegates update to repository.
    /// </summary>
    /// <param name="id">The ID of the game to update</param>
    /// <param name="updateDto">The updated video game data from API request</param>
    /// <returns>Updated VideoGameDto if successful, null if game not found</returns>
    Task<VideoGameDto?> UpdateAsync(int id, UpdateVideoGameDto updateDto);

    /// <summary>
    /// Deletes a video game from the catalogue.
    /// Service delegates to repository and returns success/failure.
    /// </summary>
    /// <param name="id">The ID of the game to delete</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(int id);
}