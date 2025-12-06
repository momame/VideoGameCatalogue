using Microsoft.EntityFrameworkCore;
using VideoGameCatalogue.Core.Entities;
using VideoGameCatalogue.Core.Interfaces;
using VideoGameCatalogue.Infrastructure.Data;

namespace VideoGameCatalogue.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for VideoGame data access operations.
/// Implements IVideoGameRepository interface - follows DIP (depends on abstraction defined in Core).
/// Handles all database interactions using Entity Framework Core.
/// Follows SRP - only responsible for data access, no business logic.
/// </summary>
public class VideoGameRepository : IVideoGameRepository
{
    private readonly VideoGameDbContext _context;

    /// <summary>
    /// Constructor with DbContext injection.
    /// DbContext is injected via DI container configured in Program.cs.
    /// Using interface in Core but implementation in Infrastructure maintains proper layering.
    /// </summary>
    public VideoGameRepository(VideoGameDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all video games ordered by title for consistent results.
    /// Using async ToListAsync for non-blocking database I/O.
    /// OrderBy ensures predictable ordering for UI display.
    /// </summary>
    public async Task<IEnumerable<VideoGame>> GetAllAsync()
    {
        return await _context.VideoGames
            .OrderBy(vg => vg.Title)  // Alphabetical ordering improves UX
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a single video game by ID.
    /// Using FindAsync optimizes for primary key lookups - checks local cache first.
    /// Returns null if not found - caller handles this scenario.
    /// </summary>
    public async Task<VideoGame?> GetByIdAsync(int id)
    {
        return await _context.VideoGames.FindAsync(id);
    }

    /// <summary>
    /// Creates a new video game in the database.
    /// Sets CreatedAt timestamp here - encapsulates audit logic in data layer.
    /// SaveChangesAsync commits the transaction and generates the ID.
    /// </summary>
    public async Task<VideoGame> CreateAsync(VideoGame videoGame)
    {
        // Set audit timestamp - repository controls this, not caller
        videoGame.CreatedAt = DateTime.UtcNow;

        // Add to DbContext's change tracker
        _context.VideoGames.Add(videoGame);

        // Persist to database - generates Id and returns affected rows count
        await _context.SaveChangesAsync();

        // Return entity with generated Id
        return videoGame;
    }

    /// <summary>
    /// Updates an existing video game in the database.
    /// Sets UpdatedAt timestamp to track modification history.
    /// Assumes entity exists - caller (service) should verify before calling.
    /// </summary>
    public async Task<VideoGame> UpdateAsync(VideoGame videoGame)
    {
        // Set audit timestamp on update
        videoGame.UpdatedAt = DateTime.UtcNow;

        // Mark entity as modified in change tracker
        _context.VideoGames.Update(videoGame);

        // Persist changes to database
        await _context.SaveChangesAsync();

        return videoGame;
    }

    /// <summary>
    /// Deletes a video game from the database.
    /// Returns false if game doesn't exist, true if deleted.
    /// Two-step process: find then delete - ensures we only delete if exists.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        // First check if game exists
        var videoGame = await _context.VideoGames.FindAsync(id);
        if (videoGame == null)
            return false;  // Not found - return false instead of throwing

        // Remove from DbContext and persist
        _context.VideoGames.Remove(videoGame);
        await _context.SaveChangesAsync();

        return true;  // Successfully deleted
    }

    /// <summary>
    /// Checks if a video game exists without loading the full entity.
    /// More efficient than GetById when you only need existence check.
    /// Useful for validation in service layer before operations.
    /// </summary>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.VideoGames.AnyAsync(vg => vg.Id == id);
    }
}