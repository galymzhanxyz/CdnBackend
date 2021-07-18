using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StaticFilesServer.Models;
using StaticFilesServer.Models.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StaticFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceActionsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly string _contentRoot;

        public ServiceActionsController(ApplicationDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _contentRoot = env.ContentRootPath;
        }

        [HttpGet]
        [Route("GetSources")]
        [DisableCors]
        public async Task<IActionResult> GetSources(Guid source_id)
        {
            string browser_id;
            var source = await _context.Source.FindAsync(source_id);

            if (source == null)
            {
                return NotFound();
            }

            if (Request.Cookies.ContainsKey("browser_id"))
            {
                browser_id = Request.Cookies["browser_id"];
            }
            else
            {
                browser_id = Guid.NewGuid().ToString();
                Response.Cookies.Append("browser_id",
                    browser_id,
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddSeconds(10 * 365 * 24 * 60 * 60),
                    });
            }

            var client_browser = await RegisterBrowserId(browser_id);
            await _context.SourceStatistic.AddAsync(new SourceStatistics
            {
                ClientBrowser = client_browser,
                Source = source,
                DownloadTime = DateTime.Now,
            });
            await _context.SaveChangesAsync();

            return Utils.Utils.GetSourceByPath(_contentRoot, source);
        }


        private async Task<ClientBrowsers> RegisterBrowserId(string browser_id)
        {
            var client_browser = _context.ClientBrowser.Where(c => c.BrowserId == browser_id).SingleOrDefault();
            if (client_browser != null)
            {
                return client_browser;
            }
            client_browser = new ClientBrowsers
            {
                ClientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                RegisterTime = DateTime.Now,
                BrowserId = browser_id,
            };
            await _context.ClientBrowser.AddAsync(client_browser);
            await _context.SaveChangesAsync();
            return client_browser;
        }
    }
}
