using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Runit.Backend.Models;
using Runit.Backend.Services;
using Runit.Backend.Infrastructure;
using System.Web;

namespace Runit.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly EmailService emailService;

        public UserController(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
        }

        // GET api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return userManager.Users.ToList();
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return new User();
        }

        // GET api/user/me
        [HttpGet("me")]
        public async Task<User> GetSelf()
        {
            var user = await userManager.GetUserAsync(User);
            return user;
        }

        // POST api/user
        [HttpPost]
        public void Post([FromBody] User value)
        {
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // POST api/user/auth
        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Authenticate([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = null;
            switch (loginDto.IdentifierType)
            {
                case LoginDto.IdentifierTypes.Email:
                    user = await userManager.FindByEmailAsync(loginDto.Identifier);
                    break;
                case LoginDto.IdentifierTypes.Id:
                    user = await userManager.FindByIdAsync(loginDto.Identifier);
                    break;
            }

            if (user == null)
            {
                return BadRequest();
            }

            var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);

                return new { Token = token };
            }

            return Unauthorized();
        }

        // POST api/user/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userManager.CreateAsync(
                new User()
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                },
                registerDto.Password
            );

            if (result.Succeeded)
            {
                return await Authenticate(new LoginDto()
                {
                    Identifier = registerDto.Email,
                    IdentifierType = LoginDto.IdentifierTypes.Email,
                    Password = registerDto.Password
                });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // POST api/user/reset_password
        [HttpPost("reset_password")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] PasswordResetDto passwordResetDto)
        {
            var user = await userManager.FindByEmailAsync(passwordResetDto.Email);

            if (!ModelState.IsValid || user == null)
            {
                return BadRequest(ModelState);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var emailSuccess = await emailService.SendPasswordResetLink(
                user.Email,
                user.Name,
                HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/reset-password/" + user.Id + "?token=" + HttpUtility.UrlEncode(token)
            );

            if (emailSuccess)
            {
                return Accepted();
            }
            else
            {
                return this.InternalServerError();
            }
        }

        // PUT api/user/reset_password
        [HttpPut("reset_password")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdatePassword([FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            var user = await userManager.FindByIdAsync(passwordUpdateDto.UserId);

            if (!ModelState.IsValid || user == null)
            {
                return BadRequest(ModelState);
            }

            var result = await userManager.ResetPasswordAsync(user, passwordUpdateDto.Token, passwordUpdateDto.Password);

            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["Authentication:Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                configuration["Authentication:Jwt:Issuer"],
                configuration["Authentication:Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginDto
        {
            [Required]
            public string Identifier { get; set; }
            [Required]
            public IdentifierTypes IdentifierType { get; set; }
            [Required]
            public string Password { get; set; }

            public enum IdentifierTypes
            {
                Email, Id
            }
        }
        public class RegisterDto
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            [Compare("Password", ErrorMessage = "The passwords do not match.")]
            public string PasswordRepeat { get; set; }
        }

        public class PasswordResetDto
        {
            [Required]
            public string Email { get; set; }
        }

        public class PasswordUpdateDto
        {
            [Required]
            public string UserId { get; set; }
            [Required]
            public string Token { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            [Compare("Password", ErrorMessage = "The passwords do not match.")]
            public string PasswordRepeat { get; set; }
        }
    }
}
