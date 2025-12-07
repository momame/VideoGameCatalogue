using Microsoft.EntityFrameworkCore;
using VideoGameCatalogue.Core.Entities;

namespace VideoGameCatalogue.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for video game catalogue.
/// Represents a session with the database - handles entity tracking and persistence.
/// Configured to use Code First approach as per assignment requirements.
/// </summary>
public class VideoGameDbContext : DbContext
{
    /// <summary>
    /// Constructor accepting DbContextOptions for dependency injection.
    /// Connection string configured in API's Program.cs - keeps infrastructure concerns separate.
    /// </summary>
    public VideoGameDbContext(DbContextOptions<VideoGameDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet representing VideoGames table in database.
    /// EF Core uses this for querying and saving VideoGame entities.
    /// </summary>
    public DbSet<VideoGame> VideoGames { get; set; }

    /// <summary>
    /// Fluent API configuration for entity mappings and database schema.
    /// Using Fluent API instead of just attributes gives more control and keeps entities clean.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure VideoGame entity
        modelBuilder.Entity<VideoGame>(entity =>
        {
            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Title - required with max length
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Optional fields with constraints
            entity.Property(e => e.Genre)
                .HasMaxLength(50);

            entity.Property(e => e.Publisher)
                .HasMaxLength(100);

            // Decimal precision for financial data (Rating and Price)
            // Using explicit column types ensures consistent precision across environments
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(3,1)");  // e.g., 9.5

            entity.Property(e => e.Price)
                .HasColumnType("decimal(10,2)");  // e.g., 59.99

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            // Audit field - required, will be set by repository
            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Optional: Add indexes for frequently queried fields (improves scalability)
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Genre);
        });

        // Seed initial data for testing and demonstration
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Seeds initial video game data for demonstration purposes.
    /// Using static DateTime values to avoid migration warnings.
    /// In production, would use migrations for data seeding or separate seed script.
    /// Provides diverse examples across different genres and price points.
    /// </summary>
    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoGame>().HasData(
            new VideoGame
            {
                Id = 1,
                Title = "The Legend of Zelda: Breath of the Wild",
                Genre = "Action-Adventure",
                ReleaseDate = new DateTime(2017, 3, 3),
                Publisher = "Nintendo",
                Rating = 9.7m,
                Price = 59.99m,
                Description = "Explore a vast open world in this critically acclaimed adventure game.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Static date
            },
            new VideoGame
            {
                Id = 2,
                Title = "God of War",
                Genre = "Action",
                ReleaseDate = new DateTime(2018, 4, 20),
                Publisher = "Sony Interactive Entertainment",
                Rating = 9.5m,
                Price = 49.99m,
                Description = "Kratos and his son Atreus embark on a perilous journey through Norse mythology.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Static date
            },
            new VideoGame
            {
                Id = 3,
                Title = "Elden Ring",
                Genre = "RPG",
                ReleaseDate = new DateTime(2022, 2, 25),
                Publisher = "FromSoftware",
                Rating = 9.3m,
                Price = 59.99m,
                Description = "A challenging action RPG set in a vast fantasy world created by FromSoftware and George R.R. Martin.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Static date
            },
            new VideoGame
            {
                Id = 4,
                Title = "FIFA 24",
                Genre = "Sports",
                ReleaseDate = new DateTime(2023, 9, 29),
                Publisher = "EA Sports",
                Rating = 8.2m,
                Price = 69.99m,
                Description = "The latest installment in the popular soccer simulation series with enhanced gameplay.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Static date
            },
            new VideoGame
            {
                Id = 5,
                Title = "Cyberpunk 2077",
                Genre = "RPG",
                ReleaseDate = new DateTime(2020, 12, 10),
                Publisher = "CD Projekt",
                Rating = 8.5m,
                Price = 39.99m,
                Description = "An open-world action-adventure RPG set in the dystopian Night City.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Static date
            },
            new VideoGame
            {
                Id = 6,
                Title = "Hades",
                Genre = "Roguelike",
                ReleaseDate = new DateTime(2020, 9, 17),
                Publisher = "Supergiant Games",
                Rating = 9.0m,
                Price = 24.99m,
                Description = "A rogue-like dungeon crawler where you defy the god of death as you hack and slash out of the Underworld.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Static date
            },

            new VideoGame
            {
                Id = 7,
                Title = "Red Dead Redemption 2",
                Genre = "Action-Adventure",
                ReleaseDate = new DateTime(2018, 10, 26),
                Publisher = "Rockstar Games",
                Rating = 9.7m,
                Price = 59.99m,
                Description = "An epic tale of life in America's unforgiving heartland with stunning visuals and deep storytelling.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 8,
                Title = "The Witcher 3: Wild Hunt",
                Genre = "RPG",
                ReleaseDate = new DateTime(2015, 5, 19),
                Publisher = "CD Projekt",
                Rating = 9.8m,
                Price = 39.99m,
                Description = "Play as Geralt of Rivia in this award-winning open-world RPG with deep story and choices that matter.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 9,
                Title = "Minecraft",
                Genre = "Sandbox",
                ReleaseDate = new DateTime(2011, 11, 18),
                Publisher = "Mojang Studios",
                Rating = 9.0m,
                Price = 26.95m,
                Description = "Build, explore, and survive in an infinite procedurally generated world made of blocks.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 10,
                Title = "Grand Theft Auto V",
                Genre = "Action-Adventure",
                ReleaseDate = new DateTime(2013, 9, 17),
                Publisher = "Rockstar Games",
                Rating = 9.5m,
                Price = 29.99m,
                Description = "Experience the story of three criminals in the sprawling city of Los Santos.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 11,
                Title = "Baldur's Gate 3",
                Genre = "RPG",
                ReleaseDate = new DateTime(2023, 8, 3),
                Publisher = "Larian Studios",
                Rating = 9.6m,
                Price = 59.99m,
                Description = "A next-generation RPG set in the Dungeons & Dragons universe with deep character customization.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 12,
                Title = "Hollow Knight",
                Genre = "Metroidvania",
                ReleaseDate = new DateTime(2017, 2, 24),
                Publisher = "Team Cherry",
                Rating = 9.2m,
                Price = 14.99m,
                Description = "A challenging 2D action-adventure through a vast interconnected underground kingdom.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 13,
                Title = "Stardew Valley",
                Genre = "Simulation",
                ReleaseDate = new DateTime(2016, 2, 26),
                Publisher = "ConcernedApe",
                Rating = 9.1m,
                Price = 14.99m,
                Description = "Escape to the countryside and create the farm of your dreams in this relaxing simulation.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 14,
                Title = "Doom Eternal",
                Genre = "FPS",
                ReleaseDate = new DateTime(2020, 3, 20),
                Publisher = "id Software",
                Rating = 8.8m,
                Price = 39.99m,
                Description = "Rip and tear through demons in this intense first-person shooter with brutal combat.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 15,
                Title = "Animal Crossing: New Horizons",
                Genre = "Simulation",
                ReleaseDate = new DateTime(2020, 3, 20),
                Publisher = "Nintendo",
                Rating = 9.0m,
                Price = 59.99m,
                Description = "Escape to a deserted island and create your own paradise in this relaxing life sim.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new VideoGame
            {
                Id = 16,
                Title = "Spider-Man: Miles Morales",
                Genre = "Action-Adventure",
                ReleaseDate = new DateTime(2020, 11, 12),
                Publisher = "Sony Interactive Entertainment",
                Rating = 8.9m,
                Price = 49.99m,
                Description = "Swing through New York City as Miles Morales with unique powers and abilities.",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}