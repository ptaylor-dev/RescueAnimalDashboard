using RescueAnimalDashboard.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("WebClient", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseResponseCompression();

app.UseCors("WebClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


var repo = new AnimalRepositoryFromCsv();

app.MapGet("/animals", async () =>
{
    var animals = await repo.GetAllAnimalsAsync();

    return animals;
}).WithName("GetAllAnimals");

app.Run();
