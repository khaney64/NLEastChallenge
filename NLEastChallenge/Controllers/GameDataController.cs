using Microsoft.AspNetCore.Mvc;

namespace NLEastChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameDataController : ControllerBase
    {
        private readonly ILogger<GameDataController> logger;

        public GameDataController(ILogger<GameDataController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            logger.LogTrace($"{nameof(GameDataController)} ctor");
        }

        [HttpGet]
        public IEnumerable<GameData> Get()
        {
            logger.LogTrace($"Start {nameof(GameDataController)}.({nameof(Get)})");
            var data = GameData.FetchTodaysGames(logger) ?? new List<GameData>();
            logger.LogTrace($"End {nameof(GameDataController)}.({nameof(Get)})");
            return data;
        }
    }
}