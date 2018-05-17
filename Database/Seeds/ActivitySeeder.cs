using System;
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
    public class ActivitySeeder : Seeder
    {
        public ActivitySeeder(RunitContext context) : base(context) { }

        public override async Task RunAsync()
        {
            var typeEasy = await context.ActivityTypes.FirstOrDefaultAsync(at => at.Name == "easy");
            var typeLong = await context.ActivityTypes.FirstOrDefaultAsync(at => at.Name == "long");
            var typeRace = await context.ActivityTypes.FirstOrDefaultAsync(at => at.Name == "race");

            var userMortimer = context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == "MORTYH@GMAIL.COM");
            var userJohnDoe = context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == "JOHN.DOE@EXAMPLE.COM");

            context.Activities.Add(new Activity()
            {
                Date = new DateTime(2018, 4, 13),
                Title = "C25K Week 6 Day 3",
                TypeId = typeLong.Id,
                Distance = 3.2,
                UserId = userMortimer.Id
            });
            context.Activities.Add(new Activity()
            {
                Date = new DateTime(2018, 4, 16),
                Title = "C25K Week 7 Day 1",
                TypeId = typeEasy.Id,
                Distance = 4,
                UserId = userMortimer.Id
            });
            context.Activities.Add(new Activity()
            {
                Date = new DateTime(2018, 4, 18),
                Title = "C25K Week 7 Day 2",
                TypeId = typeEasy.Id,
                Distance = 4,
                UserId = userMortimer.Id
            });
            context.Activities.Add(new Activity()
            {
                Date = new DateTime(2018, 5, 5),
                Title = "Lundaloppet 10k",
                TypeId = typeRace.Id,
                Distance = 10,
                UserId = userMortimer.Id
            });
            context.Activities.Add(new Activity()
            {
                Date = new DateTime(2018, 5, 05),
                Title = "Lundaloppet 5k",
                TypeId = typeRace.Id,
                Distance = 5,
                UserId = userJohnDoe.Id
            });

            await context.SaveChangesAsync();
        }
    }
}