using System.Net.Http.Headers;
using System.Net.Http.Json;
using MarketingAlliance.Core.Entities.Request;
using MarketingAlliance.Core.Entities.Response;
using Microsoft.Extensions.Logging;

namespace MarketingAlliance.App.Service
{
    public class RetailsService
    {
        private readonly ILogger<RetailsService> _logger;
        private const string _host = "https://api.garzdrav.ru:7090";

        public RetailsService(ILogger<RetailsService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<RetailCoordinates>> GetRetailCoordinates(string token, Guid[] retailTypeGuids, Guid[] statusGuids)

        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await client.PostAsJsonAsync(
                    requestUri: $"{_host}/v1/retails/api/Retails",
                    value: new RetailRequestDto
                    {
                        RetailTypeGuids = retailTypeGuids,
                        StatusGuids = statusGuids
                    });
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<RetailDto>>();
                    return result.Select(r => new RetailCoordinates
                    {
                        Id = r.RetailGuid,
                        CoordinateX = r.Coordinates[0],
                        CoordinateY = r.Coordinates[1]
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting retail coordinates");
            }

            return Enumerable.Empty<RetailCoordinates>();
        }
    }
}