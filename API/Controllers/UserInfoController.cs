using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public UserInfoController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInformation>>> GetUserInfos()
        {
            var UserInfos = await _context.UserInformations.Include(user => user.Stall).ToListAsync();
            return UserInfos;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserInformation>> GetInfo(int id)
        {
            var info = await _context.UserInformations.Where(user=>user.Id==id).Include(user=>user.Stall).FirstOrDefaultAsync();
            return info;
        }
        [HttpPost]
        public async Task<IActionResult> PostUserInformation([FromBody] LesseeRequest info)
        {
            var stall = await _context.Stalls.Where(stall => stall.StallNumber == info.StallNumber).FirstOrDefaultAsync();
            if (stall == null)
            {
                return NotFound();
            }
            UserInformation user = new UserInformation
            {
                Stall = stall,
                StallId = stall.Id,
                FirstName=info.FirstName,
                LastName=info.LastName,
                MiddleInitial=info.MiddleInitial,
                Age=info.Age,
                Sex=info.Sex,
                Address=info.Address,
                ContactNumber=info.ContactNumber,
                Email=info.Email,
                Status=info.Status
            };
            _context.UserInformations.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInfo", new { id =user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInfo(int id, UserInformation info)
        {
            if (id != info.Id)
            {
                return BadRequest();
            }

            _context.Entry(info).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InfoExists(id))
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
          private bool InfoExists(int id)
        {
             return _context.UserInformations.Any(e => e.Id == id);
        }

    }
}

