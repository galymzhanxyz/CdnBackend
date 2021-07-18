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
    public class SourcesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public SourcesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Sources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sources>>> GetSource()
        {
            return await _context.Source.ToListAsync();
        }

        // GET: api/Sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sources>> GetSources(Guid id)
        {
            var sources = await _context.Source.FindAsync(id);

            if (sources == null)
            {
                return NotFound();
            }

            return sources;
        }

        // PUT: api/Sources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSources(Guid id, Sources sources)
        {
            if (id != sources.Id)
            {
                return BadRequest();
            }

            _context.Entry(sources).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourcesExists(id))
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

        // POST: api/Sources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sources>> PostSources(Sources sources)
        {
            _context.Source.Add(sources);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSources", new { id = sources.Id }, sources);
        }

        // DELETE: api/Sources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSources(Guid id)
        {
            var sources = await _context.Source.FindAsync(id);
            if (sources == null)
            {
                return NotFound();
            }

            _context.Source.Remove(sources);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SourcesExists(Guid id)
        {
            return _context.Source.Any(e => e.Id == id);
        }
    }
}
