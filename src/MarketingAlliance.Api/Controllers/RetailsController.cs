using MarketingAlliance.App.Service;
using MarketingAlliance.Core;
using MarketingAlliance.Core.Entities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarketingAlliance.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class RetailsController : ControllerBase
    {
        private readonly RetailsService _retailsService;
        private readonly CacheService _cacheService;
        private readonly string _cacheKey = "retails-coordinates:";

        public RetailsController(RetailsService retailsService, CacheService cacheService)

        {
            _retailsService = retailsService;
            _cacheService = cacheService;
        }

        [HttpGet("retails/coordinates"), Authorize]
        [SwaggerOperation(Summary = "Получение координат открытых аптек")]
        [SwaggerResponse(200, "Успешный ответ", Type = typeof(List<RetailCoordinates>), ContentTypes = new[] { "application/json" })]
        [SwaggerResponse(204, "Нет данных")]
        public async Task<IActionResult> GetRetailCoordinates([FromHeader(Name = "Authorization")] string token)
        {
            // Guid sessionGuid = Guid.Parse(HttpContext.Request.Headers["X-Session-Guid"]);

            var cachedRetailCoordinates = await _cacheService.GetAsync<List<RetailCoordinates>>(_cacheKey);
            if (cachedRetailCoordinates != null)
                return Ok(cachedRetailCoordinates);

            var retailCoordinates = await _retailsService.GetRetailCoordinates(token.Replace("Bearer ", ""),
                                                                               new Guid[] { Constants.PharmacyRetailTypeGuid },
                                                                               new Guid[] { Constants.OpenedRetailStatusGuid });

            await _cacheService.SetAsync(_cacheKey, retailCoordinates, TimeSpan.FromHours(2), TimeSpan.FromHours(4));
            return Ok(retailCoordinates);
        }
    }
}