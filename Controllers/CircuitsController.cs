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
    public class CircuitsController : ControllerBase
    {
        private readonly LtmsContext _context;

        public CircuitsController(LtmsContext context)
        {
            _context = context;
        }

        // GET: api/Circuits
        [HttpGet("GetAllCircuits")]
        public async Task<ActionResult<IEnumerable<Circuit>>> GetAllCircuits()
        {
          if (_context.Circuits == null)
          {
              return NotFound();
          }
            return await _context.Circuits.ToListAsync();
        }

        // GET: api/Circuits/5
        [HttpGet("GetCircuit")]
        public async Task<ActionResult<Circuit>> GetCircuit(int id)
        {
          if (_context.Circuits == null)
          {
              return NotFound();
          }
            var circuit = await _context.Circuits.FindAsync(id);

            if (circuit == null)
            {
                return NotFound();
            }

            return circuit;
        }

        // PUT: api/Circuits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("PutCircuit")]
        public async Task<IActionResult> PutCircuit(int id, Circuit circuit)
        {
            if (id != circuit.Id)
            {
                return BadRequest();
            }

            _context.Entry(circuit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CircuitExists(id))
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

        // POST: api/Circuits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostCircuit")]
        public async Task<ActionResult<Circuit>> PostCircuit(Circuit circuit)
        {
            if (string.IsNullOrEmpty(circuit.Agence))
            {
                return BadRequest("circuit property is required.");
            }


            var agence = await _context.Agences.FirstOrDefaultAsync(a => a.Nom == circuit.Agence);
            if (agence == null)
            {
                return BadRequest("circuit not found.");
            }

            circuit.AgenceNavigation = agence;
            _context.Circuits.Add(circuit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCircuit", new { id = circuit.Id }, circuit);
        }

        // DELETE: api/Circuits/5
        [HttpDelete("DeleteCircuit")]
        public async Task<IActionResult> DeleteCircuit(int id)
        {
            if (_context.Circuits == null)
            {
                return NotFound();
            }
            var circuit = await _context.Circuits.FindAsync(id);
            if (circuit == null)
            {
                return NotFound();
            }

            _context.Circuits.Remove(circuit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CircuitExists(int id)
        {
            return (_context.Circuits?.Any(e => e.Id == id)).GetValueOrDefault();
        }

       
    }
}
