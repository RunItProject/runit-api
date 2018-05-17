using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Runit.Backend.Models;

namespace Runit.Backend.Database.Seeds
{
    public class UserSeeder : Seeder
    {
        private readonly UserManager<User> userManager;

        public UserSeeder(RunitContext context, UserManager<User> userManager) : base(context) {
            this.userManager = userManager;
        }

        public override async Task RunAsync()
        {
            await userManager.CreateAsync(new User(){
                Email = "jane.doe@example.com",
                Name =  "Jane Doe"
            });

            await userManager.CreateAsync(new User(){
                Email = "john.doe@example.com",
                Name = "John Doe"
            });
        }
    }
}