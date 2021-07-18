using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public StatisticsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetCountDownloaded")]
        public IEnumerable<CountDownloadeds> GetCountDownloaded()
        {
            return Utils.Utils.GetCountDownloadeds(_context.Database.GetDbConnection());
        }

        [HttpGet]
        [Route("GetLastDownloaded")]
        public IEnumerable<LastDownloadeds> GetLastDownloaded()
        {
            return Utils.Utils.GetLastDownloadeds(_context.Database.GetDbConnection());
        }
    }
}
