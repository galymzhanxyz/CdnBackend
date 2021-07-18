using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaticFilesServer.Models;
using StaticFilesServer.Models.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceStatisticsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public SourceStatisticsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/SourceStatistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SourceStatistics>>> GetSourceStatistic()
        {
            return await _context.SourceStatistic.ToListAsync();
        }

        // GET: api/SourceStatistics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SourceStatistics>> GetSourceStatistics(int id)
        {
            var sourceStatistics = await _context.SourceStatistic.FindAsync(id);

            if (sourceStatistics == null)
            {
                return NotFound();
            }

            return sourceStatistics;
        }

        // PUT: api/SourceStatistics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSourceStatistics(int id, SourceStatistics sourceStatistics)
        {
            if (id != sourceStatistics.Id)
            {
                return BadRequest();
            }

            _context.Entry(sourceStatistics).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceStatisticsExists(id))
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

        // POST: api/SourceStatistics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SourceStatistics>> PostSourceStatistics(SourceStatistics sourceStatistics)
        {
            _context.SourceStatistic.Add(sourceStatistics);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSourceStatistics", new { id = sourceStatistics.Id }, sourceStatistics);
        }

        // DELETE: api/SourceStatistics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSourceStatistics(int id)
        {
            var sourceStatistics = await _context.SourceStatistic.FindAsync(id);
            if (sourceStatistics == null)
            {
                return NotFound();
            }

            _context.SourceStatistic.Remove(sourceStatistics);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SourceStatisticsExists(int id)
        {
            return _context.SourceStatistic.Any(e => e.Id == id);
        }
    }
}
