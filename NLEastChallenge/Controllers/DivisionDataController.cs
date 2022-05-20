﻿using Microsoft.AspNetCore.Mvc;
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
            logger.LogTrace($"{nameof(DivisionDataController)} ctor");
        }

        [HttpGet]
        public DivisionDataVm Get()
        {
            logger.LogTrace($"{nameof(DivisionDataController)}.{nameof(Get)}()");
            try
            {
                if (!cache.TryGetValue("division", out DivisionDataVm? data))
                {
                    logger.LogTrace("Cache miss, fetching game data");
                    data = DivisionData.GetData(configuredData, logger);
                    cache.Set("division", data, TimeSpan.FromMinutes(1));
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