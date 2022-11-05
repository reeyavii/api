using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly INotificationService _notificationService;
        public UserInfoController(DatabaseContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInformation>>> GetUserInfos()
        {
            var UserInfos = await _context.UserInformations.Include(user => user.Stall).ToListAsync();
            return UserInfos;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserInformation>> GetInfo(string id)
        {
            var info = await _context.UserInformations.Where(user=>user.UserId==id).Include(user=>user.Stall).FirstOrDefaultAsync();
            return info;
        }
        [HttpGet("lessee/{id}")]
        public async Task<ActionResult<UserInformation>> GetInfoId(int id)
        {
            var info = await _context.UserInformations.Where(user => user.Id == id).Include(user => user.Stall).FirstOrDefaultAsync();
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
                Status=info.Status,
                CivilStatus=info.CivilStatus,
                Brgy=info.Brgy,
                Municipality=info.Municipality,
                ZipCode=info.ZipCode,
                Province=info.Province,
                UserId=info.UserId,

    };
            string fullName = info.FirstName + " " + info.LastName;
            var notification = _notificationService.SendNotif(fullName, NotificationType.RequestApplicationApproval, "Request Application Approval");

            _context.UserInformations.Add(user);
            _context.Notifications.Add(notification);
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

