#nullable disable
using API.Auth;
using API.Models;
using API.Sms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly ISmsService _sms;

        public AuthController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DatabaseContext context,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration, ISmsService sms)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _sms = sms;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var userId = new Guid(user.Id);
                var token = GetToken(authClaims);
               
                
                bool isAdmin = false;
                if (userRoles.Contains("Admin"))
                {
                    isAdmin = true;
                }

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAdmin = isAdmin,
                    verified = user.Verified,
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleInitial = model.MiddleInitial,
                Age = model.Age,
                Sex = model.Sex,
                Status = model.Status,
                Address = model.Address,
            };
            Profile profile = new()
            {
                UserId = user.Id,
                Email = model.Email,
                Address = model.Address,
                ImageUrl = "",
            };

            _context.Profiles.Add(profile);
            // TBA: password check

            var result = await _userManager.CreateAsync(user, model.Password);
            
            
            await _context.SaveChangesAsync();

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));


            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            
            
            
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
         [Authorize]
        [HttpPost]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] Password model)
        {
            if (User.Identity != null)
            {
              var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (
                    result.Succeeded == true 
                    )
                {
                    return Ok("Update Password Success");
                } 
                else 
                { return BadRequest("Password Error"); }
            }
            return BadRequest("Password Error");
        }

        [HttpPost]
        [Route("update-number-request")]
        public async Task<IActionResult> UpdateNumber([FromBody] PhoneNumber model)
        {
            if (User.Identity != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                //user.PhoneNumber = model.Number;
                //await _userManager.UpdateAsync(user);
                //await _context.SaveChangesAsync();
                //if (user != null) 
                //{
                //    return Ok();
                //}    
                //else
                //{ return BadRequest("Change Error"); }
                string phoneNumber = model.Number;
                if (phoneNumber.Count() == 11)
                {
                    phoneNumber = phoneNumber[1..];
                    phoneNumber = "+63" + phoneNumber;
                }
                
                
                DateTime dt = DateTime.Now;

                Random r = new Random();
                var x = r.Next(0, 1000000);
                string pin = x.ToString("000000");
                Verification verification = new Verification
                {
                    UserId = user.Id,
                    Pin = pin,
                    IssuedDateTime = dt,
                    ExpirationDateTime = dt.AddMinutes(10),
                };
                _context.Verifications.Add(verification);
                await _context.SaveChangesAsync();

                await _sms.SendPin(pin, phoneNumber);


                return Ok();
            }
            return BadRequest("Change Error");
        }

        [HttpPost]
        [Route("update-number-verified/{phoneNumber}")]
        public async Task<IActionResult> UpdateNumberVerified(string phoneNumber,[FromBody] Verify verify)
        {

            var verification = await _context.Verifications.Where(ver => ver.UserId == verify.UserId && verify.RequestDateTime < ver.ExpirationDateTime && verify.RequestDateTime > ver.IssuedDateTime).OrderByDescending(ver => ver.IssuedDateTime).FirstOrDefaultAsync();
            if (verification.Pin == verify.Pin)
            {
                var user = await _userManager.FindByIdAsync(verify.UserId);
                user.PhoneNumber = phoneNumber;
                _context.Entry(user).State = EntityState.Modified;
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
            return BadRequest();
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdmin model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleInitial = model.MiddleInitial,
                EmployeeId = model.EmployeeId,
                Address = model.Address,

            };

            var result = await _userManager.CreateAsync(user, model.Password);
            //create store context
          
            await _context.SaveChangesAsync();

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "Store Admin created successfully!" });
        }



        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        [HttpGet]
        [Route("request-verification/{id}")]
        public async Task<IActionResult> RequestPin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            string phoneNumber = user.PhoneNumber;
            if (phoneNumber.Count() == 11)
            {
                phoneNumber = phoneNumber[1..];
            }
            phoneNumber = "+63" + phoneNumber;
            DateTime dt = DateTime.Now;

            Random r = new Random();
            var x = r.Next(0, 1000000);
            string pin = x.ToString("000000");
            Verification verification = new Verification
            {
                UserId = id,
                Pin = pin,
                IssuedDateTime = dt,
                ExpirationDateTime = dt.AddMinutes(10),
            };
            _context.Verifications.Add(verification);
            await _context.SaveChangesAsync();

            await _sms.SendPin(pin, phoneNumber);


            return Ok();
        }

        [HttpPost]
        [Route("verify")]
        public async Task<IActionResult> Verify(Verify verify)
        {
            var verification = await _context.Verifications.Where(ver => ver.UserId == verify.UserId && verify.RequestDateTime < ver.ExpirationDateTime && verify.RequestDateTime > ver.IssuedDateTime).OrderByDescending(ver => ver.IssuedDateTime).FirstOrDefaultAsync();
            if (verification.Pin == verify.Pin)
            {
                var user = await _userManager.FindByIdAsync(verify.UserId);
                user.Verified = true;
                _context.Entry(user).State = EntityState.Modified;
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
            return BadRequest();
        }
        [HttpPost]
        [Route("forgot-password-verify")]
        public async Task<IActionResult> ForgotPassRequest(ForgotPasswordVerify forgotPassVerify)
        {
            var user = await _userManager.FindByNameAsync(forgotPassVerify.Username);
            var verification = await _context.ForgotPasswordVerifications.Where(ver => ver.Username == forgotPassVerify.Username && forgotPassVerify.RequestDateTime < ver.ExpirationDateTime && forgotPassVerify.RequestDateTime > ver.IssuedDateTime).OrderByDescending(ver => ver.IssuedDateTime).FirstOrDefaultAsync();
            if (user != null && verification.Pin == forgotPassVerify.Pin)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var userId = new Guid(user.Id);
                var token = GetToken(authClaims);


                bool isAdmin = false;
                if (userRoles.Contains("Admin"))
                {
                    isAdmin = true;
                }

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAdmin = isAdmin,
                    verified = user.Verified,
                });
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("request-forgot-password/{username}")]
        public async Task<IActionResult> RequestForgotPass(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            string phoneNumber = user.PhoneNumber;
            if (phoneNumber.Count() == 11)
            {
                phoneNumber = phoneNumber[1..];
            }
            phoneNumber = "+63" + phoneNumber;
            DateTime dt = DateTime.Now;

            Random r = new Random();
            var x = r.Next(0, 1000000);
            string pin = x.ToString("000000");
            ForgotPasswordVerification verification = new ForgotPasswordVerification
            {
                Username = username,
                Pin = pin,
                IssuedDateTime = dt,
                ExpirationDateTime = dt.AddMinutes(10),
            };
            _context.ForgotPasswordVerifications.Add(verification);
            await _context.SaveChangesAsync();

            await _sms.SendPin(pin, phoneNumber);


            return Ok();
        }



    }
}