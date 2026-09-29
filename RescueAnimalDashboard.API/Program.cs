using RescueAnimalDashboard.Infrastructure.Persistence.Context;
using RescueAnimalDashboard.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RescueAnimalDashboard.Domain.Interfaces;

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

app.UseCors("WebClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/animals", async (IAnimalRepository repository, CancellationToken ct) =>
    Results.Ok(await repository.GetAllAnimalsAsync(ct)));

// Apply migrations; the DbContext's seeding hooks run afterwards
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RescueAnimalDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
