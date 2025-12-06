using Microsoft.AspNetCore.Mvc;
using VideoGameCatalogue.Core.DTOs;
using VideoGameCatalogue.Core.Interfaces;

namespace VideoGameCatalogue.API.Controllers;

/// <summary>
/// API controller for video game catalogue operations.
/// Handles HTTP requests/responses only - delegates business logic to service layer.
/// Follows SRP - responsible for HTTP concerns (routing, status codes, request/response handling).
/// RESTful design - uses proper HTTP verbs and status codes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VideoGamesController : ControllerBase
{
    private readonly IVideoGameService _service;
    private readonly ILogger<VideoGamesController> _logger;

    /// <summary>
    /// Constructor with service and logger injection.
    /// Using IVideoGameService interface (DIP) - controller doesn't know about repository or DbContext.
    /// Logger for tracking errors and monitoring production behavior.
    /// </summary>
    public VideoGamesController(
        IVideoGameService service,
        ILogger<VideoGamesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/videogames
    /// Retrieves all video games in the catalogue.
    /// Returns 200 OK with list of games (empty list if none exist).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VideoGameDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VideoGameDto>>> GetAll()
    {
        try
        {
            var games = await _service.GetAllAsync();
            return Ok(games);
        }
        catch (Exception ex)
        {
            // Log error for monitoring/debugging - don't expose internal details to client
            _logger.LogError(ex, "Error retrieving all video games");

            // Return generic error message - security best practice
            return StatusCode(500, "An error occurred while retrieving video games");
        }
    }

    /// <summary>
    /// GET: api/videogames/{id}
    /// Retrieves a specific video game by ID.
    /// Returns 200 OK with game if found, 404 Not Found if doesn't exist.
    /// </summary>
    /// <param name="id">The unique identifier of the video game</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VideoGameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VideoGameDto>> GetById(int id)
    {
        try
        {
            var game = await _service.GetByIdAsync(id);

            // Service returns null if not found - translate to HTTP 404
            if (game == null)
            {
                _logger.LogWarning("Video game with ID {Id} not found", id);
                return NotFound($"Video game with ID {id} not found");
            }

            return Ok(game);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving video game with ID {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the video game");
        }
    }

    /// <summary>
    /// POST: api/videogames
    /// Creates a new video game in the catalogue.
    /// Returns 201 Created with location header pointing to new resource.
    /// Returns 400 Bad Request if validation fails.
    /// </summary>
    /// <param name="createDto">The video game data to create</param>
    [HttpPost]
    [ProducesResponseType(typeof(VideoGameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VideoGameDto>> Create([FromBody] CreateVideoGameDto createDto)
    {
        try
        {
            // Model validation happens automatically via data annotations
            // If invalid, returns 400 with validation errors automatically
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Delegate creation to service layer
            var created = await _service.CreateAsync(createDto);

            // Return 201 Created with Location header (RESTful best practice)
            // CreatedAtAction generates URL: api/videogames/{id}
            return CreatedAtAction(
                nameof(GetById),           // Action name for Location header
                new { id = created.Id },   // Route values
                created                     // Response body
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating video game with title {Title}", createDto.Title);
            return StatusCode(500, "An error occurred while creating the video game");
        }
    }

    /// <summary>
    /// PUT: api/videogames/{id}
    /// Updates an existing video game.
    /// Returns 200 OK with updated game if successful, 404 Not Found if doesn't exist.
    /// Returns 400 Bad Request if validation fails.
    /// </summary>
    /// <param name="id">The ID of the video game to update</param>
    /// <param name="updateDto">The updated video game data</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VideoGameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VideoGameDto>> Update(int id, [FromBody] UpdateVideoGameDto updateDto)
    {
        try
        {
            // Validate request body
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Delegate update to service - service checks if exists
            var updated = await _service.UpdateAsync(id, updateDto);

            // Service returns null if not found - translate to 404
            if (updated == null)
            {
                _logger.LogWarning("Attempted to update non-existent video game with ID {Id}", id);
                return NotFound($"Video game with ID {id} not found");
            }

            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating video game with ID {Id}", id);
            return StatusCode(500, "An error occurred while updating the video game");
        }
    }

    /// <summary>
    /// DELETE: api/videogames/{id}
    /// Deletes a video game from the catalogue.
    /// Returns 204 No Content if successful, 404 Not Found if doesn't exist.
    /// </summary>
    /// <param name="id">The ID of the video game to delete</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            // Delegate deletion to service
            var deleted = await _service.DeleteAsync(id);

            // Service returns false if not found - translate to 404
            if (!deleted)
            {
                _logger.LogWarning("Attempted to delete non-existent video game with ID {Id}", id);
                return NotFound($"Video game with ID {id} not found");
            }

            // Return 204 No Content - successful deletion with no response body
            // RESTful convention for DELETE operations
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting video game with ID {Id}", id);
            return StatusCode(500, "An error occurred while deleting the video game");
        }
    }
}