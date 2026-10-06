using RescueAnimalDashboard.Application.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RescueAnimalDashboard.Web.Services.Api;

// This context class should deserialize JSON objects
// faster than default to improve client side performance
[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(AnimalResponse))]
internal partial class AppJsonContext : JsonSerializerContext
{
}