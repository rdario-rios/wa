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
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await _context.Contacts
                .AsNoTracking()
                .AsQueryable()
                .Include(m => m.Skills).ThenInclude(e => e.SkillDefinition)
                .Include(m => m.Skills).ThenInclude(e => e.SkillLevel)
                .ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts
                .AsNoTracking()
                .AsQueryable()
                .Include(m => m.Skills).ThenInclude(e => e.SkillDefinition)
                .Include(m => m.Skills).ThenInclude(e => e.SkillLevel)
                .FirstOrDefaultAsync(i => i.ContactID == id);
                //.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.ContactID)
            {
                return BadRequest();
            }
            if (ContactEmailUsed(contact.ContactID, contact.Email))
            {
                return Conflict();
            }
            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contacts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            if (_context.Contacts.ToList().Count > 0)
                contact.ContactID = _context.Contacts.ToList().Max(c => c.ContactID) + 1;
            else
                contact.ContactID = 0;

            if (ContactEmailExists(contact.Email))
                return Conflict("This email address is already assigned in the contact book");
            _context.Contacts.Add(contact);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ContactExists(contact.ContactID))
                {
                    return Conflict();
                }
                else
                {
                    throw new InvalidOperationException("Error while recording in the database");
                }
            }

            return CreatedAtAction("GetContact", new { id = contact.ContactID }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(int id)
        {
            // TODO : Remove Skills in DB
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        // POST: api/Contacts/5
        [HttpPost("{id}/Skills")]
        public async Task<ActionResult<Contact>> AddSkill(int id, Skill skill)
        {
            if (id != skill.ContactID || !ContactExists(id) || !SkillDefinitionExists(skill.SkillDefinitionID) || 
                !SkillLevelExists(skill.SkillLevelID) || SkillDefinitionAlreadyAssignedToThisContact(id, skill.SkillDefinitionID))
            {
                return BadRequest();
            }

            if (_context.Skills.ToList().Count > 0)
                skill.SkillID = _context.Skills.ToList().Max(s => s.SkillID) + 1;
            else
                skill.SkillID = 0;

            _context.Skills.Add(skill);

            await _context.SaveChangesAsync();
            var contact = await _context.Contacts
                .AsNoTracking()
                .AsQueryable()
                .Include(m => m.Skills).ThenInclude(e => e.SkillDefinition)
                .Include(m => m.Skills).ThenInclude(e => e.SkillLevel)
                .FirstOrDefaultAsync(i => i.ContactID == id);

            return contact;
        }

        // PUT: api/Contacts/5/Skills/10
        [HttpPut("{id}/Skills/{skillID}")]
        public async Task<ActionResult<Contact>> UpdateLevelSkill(int id, int skillID, Skill skill)
        {
            
            if (!SkillAssignedToThisContact(id, skillID) || skillID != skill.SkillID || SkillDefinitionModifiedInUpdateSkillLevel(skillID, skill.SkillDefinitionID))
            {
                return BadRequest("INFORMATION");
            }

            _context.Entry(skill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var contact = await _context.Contacts
                .AsNoTracking()
                .AsQueryable()
                .Include(m => m.Skills).ThenInclude(e => e.SkillDefinition)
                .Include(m => m.Skills).ThenInclude(e => e.SkillLevel)
                .FirstOrDefaultAsync(i => i.ContactID == id);

            return contact;
        }

        // DELETE: api/Contacts/5/Skills/10
        [HttpDelete("{id}/Skills/{skillID}")]
        public async Task<ActionResult<Contact>> RemoveSkill(int id, int skillID)
        {

            if (!SkillAssignedToThisContact(id, skillID))
            {
                return BadRequest();
            }
            var skill = await _context.Skills.FindAsync(skillID);
            if (skill == null)
            {
                return NotFound();
            }
            _context.Skills.Remove(skill);

            await _context.SaveChangesAsync();

            var contact = await _context.Contacts
                .AsNoTracking()
                .AsQueryable()
                .Include(m => m.Skills).ThenInclude(e => e.SkillDefinition)
                .Include(m => m.Skills).ThenInclude(e => e.SkillLevel)
                .FirstOrDefaultAsync(i => i.ContactID == id);

            return contact;
        }

        private bool SkillDefinitionModifiedInUpdateSkillLevel(int skillID, int skillDefitionID)
        {
            return _context.Skills.Any(s => s.SkillID == skillID && s.SkillDefinitionID != skillDefitionID);
        }
        private bool SkillAssignedToThisContact(int contactID, int skillID)
        {
            return _context.Skills.Any(s => s.ContactID == contactID && s.SkillID == skillID);
        }
        private bool SkillDefinitionAlreadyAssignedToThisContact(int contactID, int skillDefinitionID)
        {
            return _context.Skills.Any(s => s.ContactID == contactID && s.SkillDefinitionID == skillDefinitionID);
        }
        private bool SkillDefinitionExists(int id)
        {
            return _context.SkillDefinitions.Any(s => s.SkillDefinitionID == id);
        }
        private bool SkillLevelExists(int id)
        {
            return _context.SkillLevels.Any(s => s.SkillLevelID == id);
        }
        private bool ContactEmailExists(string email)
        {
            return _context.Contacts.Any(e => e.Email == email);
        }
        private bool ContactEmailUsed(int id, string email)
        {
            return _context.Contacts.Any(e => e.Email == email && e.ContactID != id);
        }
        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactID == id);
        }
    }
}
