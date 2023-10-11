using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polyclinic_Schedule.DB;
using Polyclinic_Schedule.DTO;

namespace Polyclinic_Schedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        private readonly User30Context _context;

        public SpecialitiesController(User30Context context)
        {
            _context = context;
        }

        // GET: api/Specialities
        [HttpGet("GetSpeciality")]
        public async Task<ActionResult<IEnumerable<DTO.Speciality>>> GetSpeciality()
        {
          if (_context.Speciality == null)
          {
              return NotFound();
          }
            return await _context.Speciality.ToListAsync();
        }

        // GET: api/Specialities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.Speciality>> GetSpeciality(int id)
        {
          if (_context.Speciality == null)
          {
              return NotFound();
          }
            var speciality = await _context.Speciality.FindAsync(id);

            if (speciality == null)
            {
                return NotFound();
            }

            return speciality;
        }

        // PUT: api/Specialities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpeciality(int id, DTO.Speciality speciality)
        {
            if (id != speciality.Id)
            {
                return BadRequest();
            }

            _context.Entry(speciality).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialityExists(id))
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

        // POST: api/Specialities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DTO.Speciality>> PostSpeciality(DTO.Speciality speciality)
        {
          if (_context.Speciality == null)
          {
              return Problem("Entity set 'User30Context.Speciality'  is null.");
          }
            _context.Speciality.Add(speciality);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpeciality", new { id = speciality.Id }, speciality);
        }

        // DELETE: api/Specialities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeciality(int id)
        {
            if (_context.Speciality == null)
            {
                return NotFound();
            }
            var speciality = await _context.Speciality.FindAsync(id);
            if (speciality == null)
            {
                return NotFound();
            }

            _context.Speciality.Remove(speciality);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpecialityExists(int id)
        {
            return (_context.Speciality?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
