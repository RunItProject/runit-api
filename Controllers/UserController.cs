using System;
using System.Collections.Generic;
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

namespace Runit.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserController(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager) {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        
        // GET api/user
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return userManager.Users.ToList();
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return new User();
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

        // POST api/auth
        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<object> Authenticate([FromBody] string email, [FromBody] string password) 
        {
            var result = await signInManager.PasswordSignInAsync(email, password, false, false);
            
            if (result.Succeeded)
            {
                var appUser = userManager.Users.SingleOrDefault(r => r.Email == email);
                return GenerateJwtToken(appUser);
            }
            
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        private object GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
