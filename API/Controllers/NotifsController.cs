using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public NotifsController( DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            var notifications = await _context.Notifications.OrderByDescending(n => n.Id).ToListAsync();
            return Ok(notifications);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Notification>>> PutNotification(int id, Notification notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }
            notification.MessageStatus = "Read";
            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var notifications = await _context.Notifications.OrderByDescending(n => n.Id).ToListAsync();

            return notifications;
        }
        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
