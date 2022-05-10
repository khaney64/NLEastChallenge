using Microsoft.AspNetCore.Mvc;

namespace NLEastChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DivisionDataController : ControllerBase
    {
        private readonly ILogger<DivisionDataController> logger;
        private readonly DivisionData[] configuredData;

        public DivisionDataController(ILogger<DivisionDataController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            configuredData = configuration.GetSection("Data").Get<DivisionData[]>();
        }

        [HttpGet]
        public DivisionDataVm Get()
        {
            return DivisionData.GetData(configuredData, logger);
        }
    }
}