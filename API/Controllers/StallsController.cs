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
       
        public static IWebHostEnvironment _webHostEnvironment;
         public StallsController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<ActionResult<Stall>> PostStall([FromForm] Stall stall )
        {
            stall.StallImage = null;
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
        public async Task<IActionResult> PutStall(int id, [FromForm] Stall stall)
        {

            if (id != stall.Id)
            {
                return BadRequest();
            }
            try
            {
                if (stall.StallImage != null)  //add condition to process without image file
                {

                    if (stall.StallImage.Length > 0)
                    {
                        string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                        if (!Directory.Exists(path))
                        {

                            Directory.CreateDirectory(path);

                        }
                        Console.WriteLine($"path={path}");
                        string fileName = "";

                        if (stall.StallImage.FileName.Contains(" "))
                        {
                            fileName = string.Join("", stall.StallImage.FileName.Split(" "));
                        }
                        else
                        {
                            fileName = stall.StallImage.FileName;
                        }

                        stall.ImageUrl = fileName;

                        using (FileStream fileStream = System.IO.File.Create(path + fileName))
                        {
                            stall.StallImage.CopyTo(fileStream);
                            fileStream.Flush();
                            _context.Entry(stall).State = EntityState.Modified;

                            try
                            {
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                throw;
                            }


                            return Ok();
                        }
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                
                else
                {
                    if (stall.Floor == null)
                    {
                        stall.Floor = "";
                    }
                    Stall newStall = new Stall
                    {
                        Id = stall.Id,
                        StallImage = stall.StallImage,
                        StallNumber = stall.StallNumber,
                        Dimension = stall.Dimension,
                        MonthlyPayment = stall.MonthlyPayment,
                        Description = stall.Description,
                        Status = stall.Status,
                        StallType = stall.StallType,
                        Mapping = stall.Mapping,
                        Floor = stall.Floor,
                        ImageUrl = stall.ImageUrl
                    };
                    
                    _context.Entry(newStall).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                        return BadRequest();
                    }

                }
            }
            catch (Exception ex)
            {
                return NoContent();
            }

        }
        private bool StallExists(int id)
        {
            return _context.Stalls.Any(e => e.Id == id);
        }

    }


}

