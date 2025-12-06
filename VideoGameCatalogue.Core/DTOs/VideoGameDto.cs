namespace VideoGameCatalogue.Core.DTOs;

/// <summary>
/// Data Transfer Object for VideoGame responses.
/// Used to separate API contract from internal domain model.
/// Excludes audit fields (CreatedAt, UpdatedAt) as they're not needed in client responses.
/// </summary>
public class VideoGameDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Genre { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public string? Publisher { get; set; }

    public decimal? Rating { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }
}