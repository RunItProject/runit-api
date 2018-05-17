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
    public class ActivityTypeSeeder : Seeder
    {
        public ActivityTypeSeeder(RunitContext context) : base(context) { }

        public override async Task RunAsync()
        {
            context.ActivityTypes.Add(new ActivityType()
            {
                Name = "easy"
            });
            context.ActivityTypes.Add(new ActivityType()
            {
                Name = "long"
            });
            context.ActivityTypes.Add(new ActivityType()
            {
                Name = "race"
            });

            await context.SaveChangesAsync();
        }
    }
}