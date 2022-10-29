using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.IO.Path;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public static IWebHostEnvironment _webHostEnvironment;
        public ProfileController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Profile>> GetProfile(string userId)
        {
            var profile = await _context.Profiles.Where(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int id,[FromForm] Profile profile)
        {

            if (id != profile.Id)
            {
                return BadRequest();
            }
            try
            {
                
                if (profile.Image.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {

                        Directory.CreateDirectory(path);

                    }
                    Console.WriteLine($"path={path}");
                    string fileName = "";

                    if (profile.Image.FileName.Contains(" "))
                    {
                        fileName = string.Join("", profile.Image.FileName.Split(" "));
                    }
                    else
                    {
                        fileName = profile.Image.FileName;
                    }

                    profile.ImageUrl = fileName;

                    using (FileStream fileStream = System.IO.File.Create(path + fileName))
                    {
                        profile.Image.CopyTo(fileStream);
                        fileStream.Flush();
                        _context.Entry(profile).State = EntityState.Modified;
                       
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
            catch (Exception ex)
            {
                return NoContent();
            }

        }
        private bool    ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }

        [HttpGet("image/{imageName}")]
        public IActionResult GetProfileImage(string imageName)
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
    }
}
