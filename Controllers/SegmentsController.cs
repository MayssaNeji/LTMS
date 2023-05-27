using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LTMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LTMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentsController : ControllerBase
    {
        private readonly LtmsContext _context;

        public SegmentsController(LtmsContext context)
        {
            _context = context;
        }

        // GET: api/Segments
        [HttpGet("GetAllSegments")]
        public async Task<ActionResult<IEnumerable<Segment>>> GetAllSegments()
        {
            if (_context.Segments == null)
            {
                return NotFound();
            }
            return await _context.Segments.ToListAsync();
        }

        // GET: api/Segments/5
        [HttpGet("GetSegment")]
        public async Task<ActionResult<Segment>> GetSegment(int id)
        {
            if (_context.Segments == null)
            {
                return NotFound();
            }
            var segment = await _context.Segments.FindAsync(id);

            if (segment == null)
            {
                return NotFound();
            }

            return segment;
        }

        // PUT: api/Segments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update"),Authorize(Roles =("Admin"))]
        public async Task<IActionResult> PutSegment(int id, [FromBody] Segment segment)
        {
            if ( segment==null)
            {
                return BadRequest();
            }

            _context.Entry(segment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SegmentExists(id))
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

        // POST: api/Segments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostSegment")]
        public async Task<ActionResult<Segment>> PostSegment(Segment segment)
        {
          if (_context.Segments == null)
          {
              return Problem("Entity set 'LtmsContext.Segments'  is null.");
          }
            _context.Segments.Add(segment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSegment", new { id = segment.Id }, segment);
        }

        // DELETE: api/Segments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteSegment(int id)
        {
           
           
            var segment = await _context.Segments.FindAsync(id);
            if (segment == null)
            {
                return NotFound();
            }

            _context.Segments.Remove(segment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SegmentExists(int id)
        {
            return (_context.Segments?.Any(e => e.Id == id)).GetValueOrDefault();
        }


       
    }
}
