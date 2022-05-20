using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace NLEastChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameDataController : ControllerBase
    {
        private readonly ILogger<GameDataController> logger;
        private readonly IMemoryCache cache;

        public GameDataController(ILogger<GameDataController> logger, IConfiguration configuration, IMemoryCache cache)
        {
            this.logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        public IEnumerable<GameData> Get()
        {
            logger.LogTrace($"{nameof(GameDataController)}.{nameof(Get)}()");
            try
            {
                if (!cache.TryGetValue("game", out List<GameData> data))
                {
                    logger.LogTrace("Cache miss, fetching game data");
                    data = GameData.FetchTodaysGames(logger) ?? new List<GameData>();
                    cache.Set("game", data, TimeSpan.FromMinutes(1));
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
    }
}