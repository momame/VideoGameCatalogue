using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VideoGameCatalogue.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "Id", "CreatedAt", "Description", "Genre", "Price", "Publisher", "Rating", "ReleaseDate", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An epic tale of life in America's unforgiving heartland with stunning visuals and deep storytelling.", "Action-Adventure", 59.99m, "Rockstar Games", 9.7m, new DateTime(2018, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Dead Redemption 2", null },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Play as Geralt of Rivia in this award-winning open-world RPG with deep story and choices that matter.", "RPG", 39.99m, "CD Projekt", 9.8m, new DateTime(2015, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Witcher 3: Wild Hunt", null },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Build, explore, and survive in an infinite procedurally generated world made of blocks.", "Sandbox", 26.95m, "Mojang Studios", 9.0m, new DateTime(2011, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minecraft", null },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Experience the story of three criminals in the sprawling city of Los Santos.", "Action-Adventure", 29.99m, "Rockstar Games", 9.5m, new DateTime(2013, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grand Theft Auto V", null },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A next-generation RPG set in the Dungeons & Dragons universe with deep character customization.", "RPG", 59.99m, "Larian Studios", 9.6m, new DateTime(2023, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Baldur's Gate 3", null },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A challenging 2D action-adventure through a vast interconnected underground kingdom.", "Metroidvania", 14.99m, "Team Cherry", 9.2m, new DateTime(2017, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hollow Knight", null },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Escape to the countryside and create the farm of your dreams in this relaxing simulation.", "Simulation", 14.99m, "ConcernedApe", 9.1m, new DateTime(2016, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stardew Valley", null },
                    { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rip and tear through demons in this intense first-person shooter with brutal combat.", "FPS", 39.99m, "id Software", 8.8m, new DateTime(2020, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doom Eternal", null },
                    { 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Escape to a deserted island and create your own paradise in this relaxing life sim.", "Simulation", 59.99m, "Nintendo", 9.0m, new DateTime(2020, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Animal Crossing: New Horizons", null },
                    { 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Swing through New York City as Miles Morales with unique powers and abilities.", "Action-Adventure", 49.99m, "Sony Interactive Entertainment", 8.9m, new DateTime(2020, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spider-Man: Miles Morales", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "VideoGames",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
