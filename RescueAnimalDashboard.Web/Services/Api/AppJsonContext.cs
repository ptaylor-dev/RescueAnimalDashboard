using RescueAnimalDashboard.Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RescueAnimalDashboard.Web.Services.Api;

// This context class should deserialize JSON objects faster than default to improve client side performance
[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(List<Animal>))]
internal partial class AppJsonContext : JsonSerializerContext
{
}