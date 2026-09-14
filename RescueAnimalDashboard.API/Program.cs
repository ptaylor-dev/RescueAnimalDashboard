using RescueAnimalDashboard.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("Open");

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
