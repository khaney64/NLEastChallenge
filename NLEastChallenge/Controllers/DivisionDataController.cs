using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace NLEastChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DivisionDataController : ControllerBase
    {
        private readonly ILogger<DivisionDataController> logger;
        private readonly DivisionData[] configuredData;
        private readonly IMemoryCache cache;

        public DivisionDataController(ILogger<DivisionDataController> logger, IConfiguration configuration, IMemoryCache cache)
        {
            this.logger = logger;
            this.cache = cache;
            configuredData = configuration.GetSection("Data").Get<DivisionData[]>();
        }

        [HttpGet]
        public DivisionDataVm Get([FromQuery] string mode = "normal")
        {
            var scoringMode = ParseScoringMode(mode);
            var cacheKey = $"division-{scoringMode}";

            logger.LogTrace($"{GetIpAddress()}|{nameof(DivisionDataController)}.{nameof(Get)}({scoringMode})");
            try
            {
                if (!cache.TryGetValue(cacheKey, out DivisionDataVm? data))
                {
                    logger.LogTrace("Cache miss, fetching game data");
                    data = DivisionData.GetData(configuredData, logger, scoringMode);
                    cache.Set(cacheKey, data, TimeSpan.FromMinutes(1));
                }
                else
                {
                    logger.LogTrace($"Cache hit");
                }
                return data;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected {nameof(Get)}() error");
                throw;
            }
        }

        private static ScoringMode ParseScoringMode(string mode)
        {
            return string.Equals(mode, "hh", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(mode, "horseshoes", StringComparison.OrdinalIgnoreCase)
                ? ScoringMode.Horseshoes
                : ScoringMode.Normal;
        }

        private string GetIpAddress()
        {
            var remoteIpAddress = Request.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            return remoteIpAddress?.ToString() ?? "";
        }
    }
}
