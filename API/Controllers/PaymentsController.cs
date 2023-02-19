using API.Models;
using API.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ISmsService _sms;
        public PaymentsController(DatabaseContext context, ISmsService sms)
        {
            _context = context;
            _sms = sms;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var payments = await _context.Payments.Include(p => p.Receipts).ToListAsync();
            return payments;
        }
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(string id)
        {
            var payment = await _context.Payments.Include(p => p.Receipts).Where(p => p.userId == id).FirstOrDefaultAsync();
            return payment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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
        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
        [HttpGet]
        [Route("delinquents")]
        public async Task<IActionResult> Delinquent()
        {
            //Recurring Job - this job is executed many times on the specified cron schedulen
            var userInfoList = await _context.UserInformations.Include(ui => ui.Stall).Where(ui => ui.Status.ToLower() == "approved").ToListAsync();

            foreach (var userInfo in userInfoList)
            {
                string phoneNumber = userInfo.ContactNumber;
                if (phoneNumber.Count() == 11)
                {
                    phoneNumber = phoneNumber[1..];
                    phoneNumber = "+63" + phoneNumber;
                }

                RecurringJob.AddOrUpdate(() => _sms.SendNotice( phoneNumber), Cron.Monthly(10));
                RecurringJob.AddOrUpdate(() => SetDelinquent(userInfo), Cron.Monthly(11));
                
            }

            return Ok();


            return Ok("offer sent!");
        }
        public async Task<UserInformation> SetDelinquent(UserInformation userInfo)
        {

            var payments = await _context.Payments.Include(p => p.Receipts).Where(p => p.userId == userInfo.UserId).FirstOrDefaultAsync();
            var latestReceiptDate = payments.Receipts[payments.Receipts.Count - 1].ReceiptDate;
            if (latestReceiptDate != null)
            {
                DateTime? latest = latestReceiptDate;
                DateTime? CurrentDate = DateTime.Now;
                var DueDate = $"{CurrentDate?.ToString("MM")}/10/{CurrentDate?.ToString("yyyy")}";
                DateTime DueDateTime = DateTime.Parse(DueDate);
                if (CurrentDate > DueDateTime)
                {
                    if (latest < DueDateTime)
                    {
                        userInfo.delinquent = "yes";
                        }
                    else
                    {
                        userInfo.delinquent = "no";
                    }
                    await _context.SaveChangesAsync();
                    // check if delinquent actually saves
                }
            }
          
            return Task(userInfo);

        }

        private UserInformation Task(UserInformation userInfo)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("delinquents/{id}")]
        public async Task<IActionResult> GetDelinquent(int id)
        {
            //Recurring Job - this job is executed many times on the specified cron schedulen
            var userInfo = await _context.UserInformations.Include(ui => ui.Stall).Where(ui => ui.Id == id).FirstOrDefaultAsync();

            string phoneNumber = userInfo.ContactNumber;
            if (phoneNumber.Count() == 11)
            {
                phoneNumber = phoneNumber[1..];
                phoneNumber = "+63" + phoneNumber;
            }
          await _sms.SendDelinquent(phoneNumber);


            return Ok();
        }

    }


}

