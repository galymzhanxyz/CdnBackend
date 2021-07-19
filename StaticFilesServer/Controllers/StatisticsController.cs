using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StaticFilesServer.Models.Context;
using StaticFilesServer.StatisticModels;
using System.Collections.Generic;

namespace StaticFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger _logger;

        public StatisticsController(ApplicationDBContext context, ILogger<StatisticsController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("Controller instantiated");
        }

        /// <summary>
        /// Get statistics of count each resuorce per browser id
        /// </summary>
        [HttpGet]
        [Route("GetCountDownloaded")]
        public IEnumerable<CountDownloadeds> GetCountDownloaded()
        {
            return Utils.Utils.GetCountDownloadeds(_context.Database.GetDbConnection());
        }

        /// <summary>
        /// Get statistics of last downloaded time for each resource per user id
        /// </summary>
        [HttpGet]
        [Route("GetLastDownloaded")]
        public IEnumerable<LastDownloadeds> GetLastDownloaded()
        {
            return Utils.Utils.GetLastDownloadeds(_context.Database.GetDbConnection());
        }
    }
}
