﻿using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.IO.Path;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notif;

        public ReceiptsController(DatabaseContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager, INotificationService notif)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _notif = notif;
        }


        [HttpGet("list/{id}")]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceipts(int id)
        {
            var receipts = await _context.Receipts.Where(r => r.PaymentId == id).OrderByDescending(r => r.Id).ToListAsync();         
            return receipts;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceiptsAdmin ()
        {
            var receipts = await _context.Receipts.OrderByDescending(r => r.Id).ToListAsync();
            foreach(Receipt receipt in receipts)
            {
            //var payment = await _context.Payments.FindAsync(id);
            //var user = await _userManager.FindByIdAsync(payment.userId);
            }  
            
            return receipts;
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetUserReceipts(string id)
        {
            var payment = await _context.Payments.Where(p => p.userId == id).FirstOrDefaultAsync();
            var receipts = await _context.Receipts.Where(r => r.PaymentId == payment.Id).OrderByDescending(r => r.Id).ToListAsync();
            return receipts;
        } 
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Receipt>> PostReceipt([FromForm] Receipt objectFile)
        {
            try
            {

                if (objectFile.Image.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {

                        Directory.CreateDirectory(path);

                    }
                    Console.WriteLine($"path={path}");
                    string fileName = "";

                    if (objectFile.Image.FileName.Contains(" "))
                    {
                        fileName = string.Join("", objectFile.Image.FileName.Split(" "));
                    }
                    else
                    {
                        fileName = objectFile.Image.FileName;
                    }
                    var receiptData = await _context.Receipts.Where(i => i.Name == fileName).FirstOrDefaultAsync();
                    var exist = receiptData != null;
                    int num = 0;
                    if (exist)
                    {
                        fileName = GetFileNameWithoutExtension(fileName) + num.ToString() + GetExtension(fileName);

                        var checkReceiptImg = await _context.Receipts.Where(i => i.Name == fileName).FirstOrDefaultAsync();
                        if (checkReceiptImg != null)
                        {
                            exist = false;
                        }
                        else
                        {
                            num++;
                        }

                    }
                    Receipt receipt = new()
                    {
                        Id = objectFile.Id,
                        PaymentId = objectFile.PaymentId,
                        RefNo = objectFile.RefNo,
                        ORNo = "",
                        Amount = objectFile.Amount,
                        ReceiptDate = objectFile.ReceiptDate,
                        Name = fileName,
                    };
                    if (User.Identity != null)
                    {
                        var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                        string fullName = user.FirstName + " " + user.LastName;
                        var notification = _notif.SendNotif(fullName, NotificationType.RequestPaymentConfirmation, "Request Payment Confirmation");
                        _context.Notifications.Add(notification);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + fileName))
                    {
                        objectFile.Image.CopyTo(fileStream);
                        fileStream.Flush();
                        _context.Receipts.Add(receipt);
                        await _context.SaveChangesAsync();
           
                        return receipt;
                    }
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
    

    [HttpGet("{id}")]
        public async Task<ActionResult<Receipt>> GetReceipt(int id)
        {
            var receipt = await _context.Receipts.FindAsync(id);
            return receipt;
        }
     [HttpGet("image/{imageName}")]
        public IActionResult GetReceiptImage(string imageName)
        {
            try
            {
                var image = System.IO.File.OpenRead(_webHostEnvironment.WebRootPath + "\\uploads\\" + imageName);
                return File(image, "image/jpeg");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Receipt>>> PutReceipt(int id, Receipt receipt)
        {
            if (id != receipt.Id)
            {
                return BadRequest();
            }
            receipt.Status = "Approved"; 
            _context.Entry(receipt).State = EntityState.Modified;
            var payment = await _context.Payments.FindAsync(receipt.PaymentId);
            payment.Balance = payment.Amount - receipt.Amount;
            payment.TotalPayment = payment.Balance + payment.Amount;
            _context.Entry(payment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiptExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var receipts = await _context.Receipts.Where(r => r.Status == receipt.Status).ToListAsync();

            return receipts;
        }
        private bool ReceiptExists(int id)
        {
            return _context.Receipts.Any(e => e.Id == id);
        }

    }


}

