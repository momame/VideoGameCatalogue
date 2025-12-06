using System.ComponentModel.DataAnnotations;

namespace VideoGameCatalogue.Core.DTOs;

/// <summary>
/// DTO for updating an existing video game.
/// Similar to CreateVideoGameDto but used for PUT operations.
/// Id comes from route parameter, not request body.
/// </summary>
public class UpdateVideoGameDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters")]
    public string? Genre { get; set; }

    public DateTime? ReleaseDate { get; set; }

    [StringLength(100, ErrorMessage = "Publisher cannot exceed 100 characters")]
    public string? Publisher { get; set; }

    [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
    public decimal? Rating { get; set; }

    [Range(0, 9999.99, ErrorMessage = "Price must be between 0 and 9999.99")]
    public decimal? Price { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }
}