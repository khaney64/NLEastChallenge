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
            logger.LogTrace($"{nameof(DivisionDataController)} ctor");
        }

        [HttpGet]
        public DivisionDataVm Get()
        {
            logger.LogTrace($"Start {nameof(DivisionDataController)}.{nameof(Get)}()");
            var data = DivisionData.GetData(configuredData, logger);
            logger.LogTrace($"End {nameof(DivisionDataController)}.{nameof(Get)}()");
            return data;
        }
    }
}