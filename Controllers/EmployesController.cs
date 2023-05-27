using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LTMS.Models;

namespace LTMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployesController : ControllerBase
    {
        private readonly LtmsContext _context;

        public EmployesController(LtmsContext context)
        {
            _context = context;
        }

        // GET: api/Employes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employe>>> GetEmployes()
        {
          if (_context.Employes == null)
          {
              return NotFound();
          }
            return await _context.Employes.ToListAsync();
        }

        // GET: api/Employes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employe>> GetEmploye(int id)
        {
          if (_context.Employes == null)
          {
              return NotFound();
          }
            var employe = await _context.Employes.FindAsync(id);

            if (employe == null)
            {
                return NotFound();
            }

            return employe;
        }

        // PUT: api/Employes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmploye(int id, Employe employe)
        {
            if (id != employe.Matricule)
            {
                return BadRequest();
            }

            _context.Entry(employe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeExists(id))
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

        // POST: api/Employes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employe>> PostEmploye(Employe employe)
        {
            if (employe.Shift == null)
            {
                return BadRequest("Shift is required.");
            }

            if (employe.Segment == null)
            {
                return BadRequest("Segment is required.");
            }
            if (employe.Station == null)
            {
                return BadRequest("Station is required.");
            }

            // Find the existing Shift entity
            var shift = await _context.Shifts.FirstOrDefaultAsync(a => a.ReferenceShift == employe.Shift);
            if (shift == null)
            {
                return BadRequest("Shift not found.");
            }

            var station = await _context.Stations.FirstOrDefaultAsync(a => a.RefSapLeoni == employe.Station);
            if (station == null)
            {
                return BadRequest("station not found.");
            }


            // Find the existing Segment entity
            var segment = await _context.Segments.FirstOrDefaultAsync(a => a.Nom == employe.Segment);
            if (segment == null)
            {
                return BadRequest("Segment not found.");
            }

            employe.ShiftNavigation = shift;
            employe.SegmentNavigation = segment;
            employe.StationNavigation = station;

            _context.Employes.Add(employe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmploye", new { id = employe.Matricule }, employe);
        }

        // DELETE: api/Employes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmploye(int id)
        {
            if (_context.Employes == null)
            {
                return NotFound();
            }
            var employe = await _context.Employes.FindAsync(id);
            if (employe == null)
            {
                return NotFound();
            }

            _context.Employes.Remove(employe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeExists(int id)
        {
            return (_context.Employes?.Any(e => e.Matricule == id)).GetValueOrDefault();
        }
    }
}
