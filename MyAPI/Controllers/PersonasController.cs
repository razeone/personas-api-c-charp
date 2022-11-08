using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly PersonaContext _context;

        public PersonasController(PersonaContext context)
        {
            _context = context;
        }

        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonaDTO>>> GetPersona()
        {
            return await _context.Persona
                .Select(x => PersonaToDTO(x))
                .ToListAsync();
        }

        // GET: api/Personas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaDTO>> GetPersona(long id)
        {
            var persona = await _context.Persona.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return PersonaToDTO(persona);
        }

        // PUT: api/Personas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersona(long id, PersonaDTO personaDto)
        {
            if (id != personaDto.Id)
            {
                return BadRequest();
            }

            var persona= await _context.Persona.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            persona.Name = personaDto.Name;
            persona.IsActive = personaDto.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PersonaExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Personas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonaDTO>> CreatePersona(PersonaDTO personaDto)
        {
            var persona = new Persona
            {
                IsActive = personaDto.IsActive,
                Name = personaDto.Name
            };

            _context.Persona.Add(persona);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPersona),
                new { id = persona.Id },
                PersonaToDTO(persona));
        }

        // DELETE: api/Personas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(long id)
        {
            var persona = await _context.Persona.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            _context.Persona.Remove(persona);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool PersonaExists(long id)
        {
            return _context.Persona.Any(e => e.Id == id);
        }

        private static PersonaDTO PersonaToDTO(Persona persona) =>
            new PersonaDTO
            {
                Id = persona.Id,
                Name = persona.Name,
                IsActive = persona.IsActive
            };
    }
}
