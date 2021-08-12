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
    public class SkillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Skills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            return await _context.Skills
                .AsNoTracking()
                .AsQueryable()
                .Include(m => m.Contact)
                .Include(m => m.SkillDefinition)
                .Include(m => m.SkillLevel)
                .ToListAsync();
        }

        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkill(int id)
        {
            var skill = await _context.Skills
                .AsNoTracking()
                .AsQueryable()
                .Include(m => m.Contact)
                .Include(m => m.SkillDefinition)
                .Include(m => m.SkillLevel)
                .FirstOrDefaultAsync(i => i.SkillID == id);

            if (skill == null)
            {
                return NotFound();
            }

            return skill;
        }

    }
}
