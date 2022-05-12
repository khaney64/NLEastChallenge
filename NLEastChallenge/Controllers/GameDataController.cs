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
        }

        [HttpGet]
        public IEnumerable<GameData> Get()
        {
            return GameData.FetchTodaysGames(logger) ?? new List<GameData>();
        }
    }
}