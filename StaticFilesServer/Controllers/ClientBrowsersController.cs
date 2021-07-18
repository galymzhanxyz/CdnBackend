using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaticFilesServer.Models;
using StaticFilesServer.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientBrowsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ClientBrowsersController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/ClientBrowsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientBrowsers>>> GetClientBrowser()
        {
            return await _context.ClientBrowser.ToListAsync();
        }

        // GET: api/ClientBrowsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientBrowsers>> GetClientBrowsers(Guid id)
        {
            var clientBrowsers = await _context.ClientBrowser.FindAsync(id);

            if (clientBrowsers == null)
            {
                return NotFound();
            }

            return clientBrowsers;
        }

        // PUT: api/ClientBrowsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientBrowsers(Guid id, ClientBrowsers clientBrowsers)
        {
            if (id != clientBrowsers.Uid)
            {
                return BadRequest();
            }

            _context.Entry(clientBrowsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientBrowsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClientBrowsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClientBrowsers>> PostClientBrowsers(ClientBrowsers clientBrowsers)
        {
            _context.ClientBrowser.Add(clientBrowsers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientBrowsers", new { id = clientBrowsers.Uid }, clientBrowsers);
        }

        // DELETE: api/ClientBrowsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientBrowsers(Guid id)
        {
            var clientBrowsers = await _context.ClientBrowser.FindAsync(id);
            if (clientBrowsers == null)
            {
                return NotFound();
            }

            _context.ClientBrowser.Remove(clientBrowsers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientBrowsersExists(Guid id)
        {
            return _context.ClientBrowser.Any(e => e.Uid == id);
        }
    }
}
