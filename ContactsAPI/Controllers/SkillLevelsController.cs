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
    public class SkillLevelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SkillLevelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SkillLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillLevel>>> GetSkillLevels()
        {
            return await _context.SkillLevels.ToListAsync();
        }

    }
}
