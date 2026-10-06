using Microsoft.EntityFrameworkCore;
using RescueAnimalDashboard.Application.Interfaces;
using RescueAnimalDashboard.Application.Models;
using RescueAnimalDashboard.Domain.Models;
using RescueAnimalDashboard.Infrastructure;
using RescueAnimalDashboard.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

// Response compression will reduce the size of the JSON payload and improve performance on the client
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

// Allows client on the same host to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebClient", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var connectionString = builder.Configuration.GetConnectionString("RescueAnimals")
    ?? throw new InvalidOperationException("Connection string 'RescueAnimals' is missing.");

builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseCors("WebClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Map API endpoints
app.MapGet("/animals", async (
    string? rescueType,
    string? searchText,
    string? sortBy,
    string? sortBySecondary,
    bool? sortAscending,
    int? pageIndex,
    int? pageSize,
    int? breedCountLimit,
    IAnimalRepository repository,
    CancellationToken ct) =>
{
    // Parse case-insensitive strings into enums
    var rescueError = TryParseEnum<RescueCategory>(
        rescueType,
        "rescueType",
        out var parsedRescueType);

    if (rescueError is not null)
        return rescueError;

    var primarySortError = TryParseEnum<AnimalSortOption>(
        sortBy,
        "sortBy",
        out var parsedSortBy);

    if (primarySortError is not null)
        return primarySortError;

    var secondarySortError = TryParseEnum<AnimalSortOption>(
        sortBySecondary,
        "sortBySecondary",
        out var parsedSortBySecondary);

    if (secondarySortError is not null)
        return secondarySortError;

    // Create the query object
    var animalQuery = new AnimalQuery(
        rescueType: parsedRescueType,
        searchText: searchText,
        sortBy: parsedSortBy,
        sortBySecondary: parsedSortBySecondary,
        sortAscending: sortAscending,
        pageIndex: pageIndex,
        pageSize: pageSize,
        breedCountLimit: breedCountLimit);

    return Results.Ok(
        await repository.GetAnimalsAsync(animalQuery, ct));
});

// Helper function to parse enum values from strings in query
static IResult? TryParseEnum<TEnum>(
    string? value,
    string parameterName,
    out TEnum? parsed)
    where TEnum : struct, Enum
{
    parsed = null;

    if (string.IsNullOrWhiteSpace(value))
        return null;

    if (Enum.TryParse<TEnum>(
            value,
            ignoreCase: true,
            out var enumValue) &&
        Enum.IsDefined(enumValue))
    {
        parsed = enumValue;
        return null;
    }

    return Results.BadRequest(new
    {
        error = $"Invalid {parameterName}.",
        allowedValues = Enum.GetNames<TEnum>()
    });
}

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RescueAnimalDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
