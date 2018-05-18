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
    public class PlanSeeder : Seeder
    {
        public PlanSeeder(RunitContext context) : base(context) { }

        public override async Task RunAsync()
        {
            var typeEasy = await context.ActivityTypes.FirstOrDefaultAsync(at => at.Name == "easy");
            var typeLong = await context.ActivityTypes.FirstOrDefaultAsync(at => at.Name == "long");
            var typeRace = await context.ActivityTypes.FirstOrDefaultAsync(at => at.Name == "race");

            var planActivities = new List<PlanActivity>
            {
                new PlanActivity() {
                    Title = "Week 1 Day 1",
                    TypeId = typeEasy.Id,
                    Distance = 3,
                    Week = 0,
                    DayOfWeek = DayOfWeek.Monday
                },
                new PlanActivity() {
                    Title = "Week 1 Day 2",
                    TypeId = typeEasy.Id,
                    Distance = 3,
                    Week = 0,
                    DayOfWeek = DayOfWeek.Wednesday
                },
                new PlanActivity() {
                    Title = "Week 1 Day 1",
                    TypeId = typeLong.Id,
                    Distance = 5,
                    Week = 0,
                    DayOfWeek = DayOfWeek.Friday
                },
                new PlanActivity() {
                    Title = "Week 2 Day 1",
                    TypeId = typeEasy.Id,
                    Distance = 4,
                    Week = 1,
                    DayOfWeek = DayOfWeek.Monday
                },
                new PlanActivity() {
                    Title = "Week 2 Day 2",
                    TypeId = typeEasy.Id,
                    Distance = 4,
                    Week = 1,
                    DayOfWeek = DayOfWeek.Wednesday
                },
                new PlanActivity() {
                    Title = "Week 2 Day 3",
                    TypeId = typeLong.Id,
                    Distance = 5,
                    Week = 1,
                    DayOfWeek = DayOfWeek.Friday
                }
            };

            var plan = new Plan()
            {
                Name = "2 week 5k plan",
                Activities = planActivities
            };

            context.Add(plan);

            await context.SaveChangesAsync();
        }
        
        public override bool ShouldRun()
        {
            return !context.PlanActivities.Any() && !context.Plans.Any();
        }
    }
}