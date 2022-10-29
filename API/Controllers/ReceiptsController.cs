using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.IO.Path;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public static IWebHostEnvironment _webHostEnvironment;
        public ReceiptsController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet("list/{id}")]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceipts(int id)
        {
            var receipts = await _context.Receipts.Where(r => r.PaymentId == id).OrderByDescending(r => r.Id).ToListAsync();
            return receipts;
        }
        
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
                    //var receipt = await _context.Receipts.Where(i => i.Name == fileName).FirstOrDefaultAsync();
                    //var exist = receipt != null;
                    //int num = 0;
                    //if (exist)
                    //{
                    //    fileName = GetFileNameWithoutExtension(fileName) + num.ToString() + GetExtension(fileName);

                    //    var checkProductImg = await _context.ProductImages.Where(i => i.Name == fileName).FirstOrDefaultAsync();
                    //    if (checkProductImg != null)
                    //    {
                    //        exist = false;
                    //    }
                    //    else
                    //    {
                    //        num++;
                    //    }
                    //}
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
        public async Task<IActionResult> PutReceipt(int id, Receipt receipt)
        {
            if (id != receipt.Id)
            {
                return BadRequest();
            }

            _context.Entry(receipt).State = EntityState.Modified;

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

            return NoContent();
        }
        private bool ReceiptExists(int id)
        {
            return _context.Receipts.Any(e => e.Id == id);
        }

    }


}

