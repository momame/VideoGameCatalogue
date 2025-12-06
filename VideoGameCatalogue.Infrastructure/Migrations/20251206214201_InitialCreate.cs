using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VideoGameCatalogue.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGames", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "Id", "CreatedAt", "Description", "Genre", "Price", "Publisher", "Rating", "ReleaseDate", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Explore a vast open world in this critically acclaimed adventure game.", "Action-Adventure", 59.99m, "Nintendo", 9.7m, new DateTime(2017, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Legend of Zelda: Breath of the Wild", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kratos and his son Atreus embark on a perilous journey through Norse mythology.", "Action", 49.99m, "Sony Interactive Entertainment", 9.5m, new DateTime(2018, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "God of War", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A challenging action RPG set in a vast fantasy world created by FromSoftware and George R.R. Martin.", "RPG", 59.99m, "FromSoftware", 9.3m, new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elden Ring", null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The latest installment in the popular soccer simulation series with enhanced gameplay.", "Sports", 69.99m, "EA Sports", 8.2m, new DateTime(2023, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "FIFA 24", null },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An open-world action-adventure RPG set in the dystopian Night City.", "RPG", 39.99m, "CD Projekt", 8.5m, new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cyberpunk 2077", null },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A rogue-like dungeon crawler where you defy the god of death as you hack and slash out of the Underworld.", "Roguelike", 24.99m, "Supergiant Games", 9.0m, new DateTime(2020, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hades", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_Genre",
                table: "VideoGames",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_Title",
                table: "VideoGames",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoGames");
        }
    }
}
