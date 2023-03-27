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
    public class AgencesController : ControllerBase
    {
        private readonly LtmsContext _context;

        public AgencesController(LtmsContext context)
        {
            _context = context;
        }

        // GET: api/Agences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agence>>> GetAgences()
        {
          if (_context.Agences == null)
          {
              return NotFound();
          }
            return await _context.Agences.ToListAsync();
        }

        // GET: api/Agences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agence>> GetAgence(string id)
        {
          if (_context.Agences == null)
          {
              return NotFound();
          }
            var agence = await _context.Agences.FindAsync(id);

            if (agence == null)
            {
                return NotFound();
            }

            return agence;
        }

        // PUT: api/Agences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgence(string id, Agence agence)
        {
            if (id != agence.Nom)
            {
                return BadRequest("Agence Introuvable");
            }

            _context.Entry(agence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenceExists(id))
                {
                    return NotFound("Agence déja existante");
                }
                else
                {
                    throw;
                }
            }

            return Ok("updated");
        }

        // POST: api/Agences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agence>> PostAgence(Agence agence)
        {


            var existingAgency = await _context.Agences.FirstOrDefaultAsync(c => c.Nom == agence.Nom);

            if (existingAgency!= null)
            {
                return BadRequest("Agence deja existante");
            }


            if (_context.Agences == null)
          {
              return Problem("Entity set 'LtmsContext.Agences'  is null.");
          }
            _context.Agences.Add(agence);
             await _context.SaveChangesAsync();
           

            return CreatedAtAction("GetAgence", new { id = agence.Nom }, agence);
        }

        // DELETE: api/Agences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgence(string id)
        {
            if (_context.Agences == null)
            {
                return NotFound();
            }
            var agence = await _context.Agences.FindAsync(id);
            if (agence == null)
            {
                return NotFound("Agence Introuvable");
            }

            _context.Agences.Remove(agence);
            await _context.SaveChangesAsync();

            return Ok("Agence supprimée");
        }

        private bool AgenceExists(string id)
        {
            return (_context.Agences?.Any(e => e.Nom == id)).GetValueOrDefault();
        }
    }
}
