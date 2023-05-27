using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LTMS.Models;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace LTMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculeController : ControllerBase
    {
        private readonly LtmsContext _context;

        public VehiculeController(LtmsContext context)
        {
            _context = context;
        }

        // GET: api/Véhicule
        [HttpGet("GetAllVehicules")]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetAllVéhicules()
        {
          if (_context.Vehicules == null)
          {
              return NotFound();
          }
            return await _context.Vehicules.ToListAsync();
        }

        // GET: api/Véhicule/5
        [HttpGet("GetVehicule")]
        public async Task<ActionResult<Vehicule>> GetVéhicule(int id)
        {
          if (_context.Vehicules == null)
          {
              return NotFound();
          }
            var véhicule = await _context.Vehicules.FindAsync(id);

            if (véhicule == null)
            {
                return NotFound();
            }

            return véhicule;
        }

        // PUT: api/Véhicule/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("PutVehicule")]
        public async Task<IActionResult> PutVehicule(int id, Vehicule véhicule)
        {
            if (id != véhicule.Id)
            {
                return BadRequest();
            }

            _context.Entry(véhicule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VéhiculeExists(id))
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

        // POST: api/Véhicule
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostVehicule")]
       
        public async Task<ActionResult<Vehicule>> PostVehicule(Vehicule vehicule)
        {
            if (string.IsNullOrEmpty(vehicule.Agence))
            {
                return BadRequest("vehicule property is required.");
            }


            var agence = await _context.Agences.FirstOrDefaultAsync(a => a.Nom == vehicule.Agence);
            if (agence == null)
            {
                return BadRequest("vehicule not found.");
            }

            vehicule.AgenceNavigation = agence;
            _context.Vehicules.Add(vehicule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicule", new { id = vehicule.Id }, vehicule);
        }

        // DELETE: api/Véhicule/5
        [HttpDelete("DeleteVehicule")]
        public async Task<IActionResult> DeleteVehicule(int id)
        {
            if (_context.Vehicules == null)
            {
                return NotFound();
            }
            var véhicule = await _context.Vehicules.FindAsync(id);
            if (véhicule == null)
            {
                return NotFound();
            }

            _context.Vehicules.Remove(véhicule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VéhiculeExists(int id)
        {
            return (_context.Vehicules?.Any(e => e.Id == id)).GetValueOrDefault();
        }

       
    }
}
