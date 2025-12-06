using Microsoft.EntityFrameworkCore;
using VideoGameCatalogue.Core.Interfaces;
using VideoGameCatalogue.Core.Services;
using VideoGameCatalogue.Infrastructure.Data;
using VideoGameCatalogue.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// SERVICE REGISTRATION (Dependency Injection Configuration)
// ====================================================================

/// <summary>
/// Configure DbContext with SQL Server.
/// Using Code First approach as required by assignment.
/// Connection string from appsettings.json for configuration flexibility.
/// </summary>
builder.Services.AddDbContext<VideoGameDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        // Enable retry on failure for transient errors (production best practice)
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

/// <summary>
/// Register repository with Scoped lifetime.
/// Scoped = one instance per HTTP request - appropriate for DbContext usage.
/// Using interface (IVideoGameRepository) for DIP - consumers depend on abstraction.
/// </summary>
builder.Services.AddScoped<IVideoGameRepository, VideoGameRepository>();

/// <summary>
/// Register service with Scoped lifetime.
/// Service layer depends on IVideoGameRepository interface, not concrete implementation.
/// DI container will inject VideoGameRepository when IVideoGameRepository is requested.
/// </summary>
builder.Services.AddScoped<IVideoGameService, VideoGameService>();

/// <summary>
/// Add API controllers to the service collection.
/// Enables MVC controller routing and model binding.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Configure CORS to allow Angular frontend to call API.
/// In production, would restrict to specific origins (e.g., https://newton.ca).
/// For development, allowing all origins for simplicity.
/// </summary>
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.AllowAnyOrigin()      // In production: .WithOrigins("http://localhost:4200")
              .AllowAnyMethod()      // Allows GET, POST, PUT, DELETE
              .AllowAnyHeader();     // Allows any request headers
    });
});

/// <summary>
/// Add Swagger/OpenAPI for API documentation and testing.
/// Swagger UI provides interactive documentation.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ====================================================================
// BUILD APPLICATION
// ====================================================================

var app = builder.Build();

// ====================================================================
// MIDDLEWARE PIPELINE CONFIGURATION
// Order matters - middlewares execute in the order they're added
// ====================================================================

/// <summary>
/// Enable Swagger UI in development environment only.
/// Provides interactive API documentation at /swagger endpoint.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Video Game Catalogue API v1");
        options.RoutePrefix = string.Empty;  // Swagger UI at root: https://localhost:xxxx/
    });
}

/// <summary>
/// Redirect HTTP requests to HTTPS for security.
/// Assignment doesn't require SSL, but it's .NET Core default and best practice.
/// </summary>
app.UseHttpsRedirection();

/// <summary>
/// Enable CORS middleware before routing.
/// Must be called before UseAuthorization and before endpoints are mapped.
/// </summary>
app.UseCors("AllowAngularApp");

/// <summary>
/// Enable authorization middleware.
/// Not using authentication for this assignment, but middleware is included by default.
/// In production mortgage app, would use Azure AD authentication here.
/// </summary>
app.UseAuthorization();

/// <summary>
/// Map controller endpoints to routes.
/// Controllers decorated with [ApiController] and [Route] attributes are automatically discovered.
/// </summary>
app.MapControllers();

// ====================================================================
// DATABASE INITIALIZATION
// Run migrations automatically on startup (development convenience)
// In production, would use separate migration scripts
// ====================================================================

/// <summary>
/// Apply pending migrations on application startup.
/// Creates database if it doesn't exist, applies all migrations.
/// Seeded data from DbContext will be inserted automatically.
/// This ensures fresh database setup for reviewers/evaluators.
/// </summary>
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VideoGameDbContext>();

        // Apply any pending migrations and create database if needed
        context.Database.Migrate();

        // Log success for visibility during development
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database migration completed successfully");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database");
        // In production, might want to fail startup if database is unavailable
        // For development, we'll continue and let developer troubleshoot
    }
}

// START APPLICATION

app.Run();