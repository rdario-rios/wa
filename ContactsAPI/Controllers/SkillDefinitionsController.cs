using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsAPI.Data;
using ContactsAPI.Models;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillDefinitionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SkillDefinitionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SkillDefinitions
        /// <summary>
        /// Get list of skills definitions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillDefinition>>> GetSkillDefinitions()
        {
            return await _context.SkillDefinitions.ToListAsync();
        }

        // GET: api/SkillDefinitions/5
        /// <summary>
        /// Get a skill definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillDefinition>> GetSkillDefinition(int id)
        {
            var skillDefinition = await _context.SkillDefinitions.FindAsync(id);

            if (skillDefinition == null)
            {
                return NotFound();
            }

            return skillDefinition;
        }

        // PUT: api/SkillDefinitions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Update the definition of a skill definition
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skillDefinition"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkillDefinition(int id, SkillDefinition skillDefinition)
        {
            // CHECK IF object id is equals to id in Url OR The definition is used in skill assigned OR Try to put a same definition of other skill definition
            if (id != skillDefinition.SkillDefinitionID || SkillDefinitionUsed(id) || SkillDefinitionDefinitionSameOther(id, skillDefinition.Definition))
            {
                return BadRequest();
            }

            _context.Entry(skillDefinition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillDefinitionExists(id))
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

        // POST: api/SkillDefinitions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Add a new skill definition
        /// </summary>
        /// <param name="skillDefinition"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<SkillDefinition>> PostSkillDefinition(SkillDefinition skillDefinition)
        {
            if (_context.SkillDefinitions.ToList().Count > 0)
                skillDefinition.SkillDefinitionID = _context.SkillDefinitions.ToList().Max(s => s.SkillDefinitionID) + 1;
            else
                skillDefinition.SkillDefinitionID = 0;

            // CHECK IF Try to create a same definition of other skill definition
            if (SkillDefinitionNameExists(skillDefinition.Definition))
            {
                return BadRequest();
            }

            _context.SkillDefinitions.Add(skillDefinition);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SkillDefinitionExists(skillDefinition.SkillDefinitionID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSkillDefinition", new { id = skillDefinition.SkillDefinitionID }, skillDefinition);
        }

        // DELETE: api/SkillDefinitions/5
        /// <summary>
        /// Remove a skill definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<SkillDefinition>> DeleteSkillDefinition(int id)
        {
            var skillDefinition = await _context.SkillDefinitions.FindAsync(id);
            if (skillDefinition == null)
            {
                return NotFound();
            }

            // CHECK IF The definition is used we can't delete this definition
            if (SkillDefinitionUsed(id))
            {
                return BadRequest();
            }

            _context.SkillDefinitions.Remove(skillDefinition);
            await _context.SaveChangesAsync();

            return skillDefinition;
        }

        private bool SkillDefinitionDefinitionSameOther(int id, string definition)
        {
            return _context.SkillDefinitions.Any(e => e.SkillDefinitionID != id && e.Definition == definition);
        }
        private bool SkillDefinitionUsed(int id)
        {
            return _context.Skills.Any(e => e.SkillDefinitionID == id);
        }
        private bool SkillDefinitionNameExists(string name)
        {
            return _context.SkillDefinitions.Any(e => e.Definition == name);
        }
        private bool SkillDefinitionExists(int id)
        {
            return _context.SkillDefinitions.Any(e => e.SkillDefinitionID == id);
        }
    }
}
