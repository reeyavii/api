using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StallsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public StallsController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Stall>> GetStall(int id)
        {
            var stall = await _context.Stalls.FindAsync(id);
            return stall;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stall>>> GetStalls() 
        { 
            var stalls = await _context.Stalls.OrderBy(stall => stall.StallNumber).ToListAsync() ;
            return stalls;
        } 
        [HttpPost]
        public async Task<ActionResult<Stall>> PostStall(Stall stall)
        {
            _context.Stalls.Add(stall);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStall", new { id = stall.Id }, stall);
        }

        [HttpGet("user-info/{id}")]
        public async Task<ActionResult<UserInformation>> GetInfo(int id)
        {
            var info = await _context.UserInformations.FindAsync(id);
            return info;
        }
        [HttpPost("user-info")]
        public async Task<ActionResult<UserInformation>> PostUserInformation(UserInformation info)
        {
            _context.UserInformations.Add(info);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInfo", new { id = info.Id }, info);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStall(int id, Stall stall)
        {
            if (id != stall.Id)
            {
                return BadRequest();
            }

            _context.Entry(stall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StallExists(id))
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
        private bool StallExists(int id)
        {
            return _context.Stalls.Any(e => e.Id == id);
        }

    }


}

