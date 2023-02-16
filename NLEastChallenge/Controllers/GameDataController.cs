using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace NLEastChallenge.Controllers
{
    [ApiController]
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
        [Route("{controller}/today")]
        public IEnumerable<GameData> Get()
        {
            logger.LogTrace($"{GetIpAddress()}|{nameof(GameDataController)}.{nameof(Get)}()");
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

        [HttpGet]
        [Route("{controller}/upcoming")]
        public IEnumerable<GameData> GetUpcoming()
        {
            logger.LogTrace($"{GetIpAddress()}|{nameof(GameDataController)}.{nameof(GetUpcoming)}()");
            try
            {
                if (!cache.TryGetValue("upcoming", out List<GameData> upcoming))
                {
                    var from = DateTime.Now.Date.AddDays(1);
                    var to = DateTime.Now.Date.AddDays(14);
                    upcoming = GameData.FetchGamesDateRanges(logger, from, to);
                    cache.Set("upcoming", upcoming, TimeSpan.FromMinutes(60 * 12));
                }
                else
                {
                    logger.LogTrace($"Cache hit");
                }
                return upcoming;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected {nameof(Get)}() error");
                throw;
            }
        }

        private string GetIpAddress()
        {
            var remoteIpAddress = Request.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            return remoteIpAddress?.ToString() ?? "";
        }
    }
}