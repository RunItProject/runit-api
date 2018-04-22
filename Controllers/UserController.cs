using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Runit.Backend.Models;

namespace Runit.Backend.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        // GET api/user
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return new List<User>();
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
        public void Authenticate(string email, string password) 
        {
            // return token
        }
    }
}
