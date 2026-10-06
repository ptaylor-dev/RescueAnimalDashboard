using Microsoft.AspNetCore.WebUtilities;
using RescueAnimalDashboard.Application.Models;
using System.Globalization;
using System.Net.Http.Json;

namespace RescueAnimalDashboard.Web.Services.Api;

public sealed class AnimalApiClient(HttpClient httpClient)
    : IAnimalApiClient
{
    public async Task<AnimalResponse> GetAnimalsAsync(
        AnimalQuery query,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var parameters = new Dictionary<string, string?>
        {
            ["rescueType"] = query.RescueType?.ToString(),
            ["searchText"] = query.SearchText,
            ["sortBy"] = query.SortBy?.ToString(),
            ["sortBySecondary"] = query.SortBySecondary?.ToString(),
            ["sortAscending"] = query.SortAscending ? "true" : "false",
            ["pageIndex"] = query.PageIndex.ToString(
                CultureInfo.InvariantCulture),
            ["pageSize"] = query.PageSize.ToString(
                CultureInfo.InvariantCulture),
            ["breedCountLimit"] = query.BreedCountLimit.ToString(
                CultureInfo.InvariantCulture)
        };

        var requestUri = QueryHelpers.AddQueryString(
            "animals",
            parameters);

        using var httpResponse = await httpClient.GetAsync(requestUri, ct);

        var statusCode = httpResponse.StatusCode;
        var responseBody = await httpResponse.Content.ReadAsStringAsync(ct);

        httpResponse.EnsureSuccessStatusCode();

        var response = await httpResponse.Content
            .ReadFromJsonAsync<AnimalResponse>(cancellationToken: ct);


        return response
            ?? throw new InvalidOperationException(
                "The animals API returned a null response.");
    }
}