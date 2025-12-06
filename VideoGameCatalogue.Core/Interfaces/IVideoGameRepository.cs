using VideoGameCatalogue.Core.Entities;

namespace VideoGameCatalogue.Core.Interfaces;

/// <summary>
/// Repository interface defining data access operations for VideoGame entity.
/// Using interface enables DIP (Dependency Inversion Principle) - controllers/services depend on abstraction, not concrete implementation.
/// This makes the code testable (can mock) and flexible (can swap SQL for NoSQL without changing consumers).
/// </summary>
public interface IVideoGameRepository
{
    /// <summary>
    /// Retrieves all video games from the database.
    /// Using async pattern for non-blocking I/O - important for scalability.
    /// </summary>
    /// <returns>Collection of all video games, ordered by title</returns>
    Task<IEnumerable<VideoGame>> GetAllAsync();

    /// <summary>
    /// Retrieves a single video game by its unique identifier.
    /// Returns null if not found - allows caller to handle 404 scenarios.
    /// </summary>
    /// <param name="id">The unique identifier of the video game</param>
    /// <returns>VideoGame if found, null otherwise</returns>
    Task<VideoGame?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new video game in the database.
    /// Repository is responsible for setting CreatedAt timestamp.
    /// </summary>
    /// <param name="videoGame">The video game entity to create (without Id)</param>
    /// <returns>The created entity with database-generated Id</returns>
    Task<VideoGame> CreateAsync(VideoGame videoGame);

    /// <summary>
    /// Updates an existing video game in the database.
    /// Repository is responsible for setting UpdatedAt timestamp.
    /// Assumes the entity already exists (caller should verify).
    /// </summary>
    /// <param name="videoGame">The video game entity with updated values</param>
    /// <returns>The updated entity</returns>
    Task<VideoGame> UpdateAsync(VideoGame videoGame);

    /// <summary>
    /// Deletes a video game from the database.
    /// Returns false if the game doesn't exist, true if successfully deleted.
    /// </summary>
    /// <param name="id">The unique identifier of the video game to delete</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Checks if a video game exists without retrieving the full entity.
    /// Useful for validation without loading unnecessary data.
    /// </summary>
    /// <param name="id">The unique identifier to check</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ExistsAsync(int id);
}