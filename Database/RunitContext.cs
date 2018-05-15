using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Runit.Backend.Models;

namespace Runit.Backend.Database
{
    public class RunitContext : IdentityDbContext<User, UserRole, int>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public RunitContext(DbContextOptions<RunitContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Activity>()
                .HasOne(activity => activity.Type)
                .WithMany()
                .HasForeignKey(activity => activity.TypeId)
                .IsRequired();
        }
        public void EnsureSeeded()
        {
            string seedsBasePath = @"Database" + Path.DirectorySeparatorChar + "Seeds" + Path.DirectorySeparatorChar;

            if (! this.Users.Any() && File.Exists(seedsBasePath + "users.json")) {
                var users = JsonConvert.DeserializeObject<List<User>>(
                    File.ReadAllText(seedsBasePath + "users.json")
                );

                if (users != null) {
                    this.AddRange(users);
                }
            }

            if (! this.ActivityTypes.Any() &&  File.Exists(seedsBasePath + "activity-types.json")) {
                var activityTypes = JsonConvert.DeserializeObject<List<ActivityType>>(
                    File.ReadAllText(seedsBasePath + "activity-types.json")
                );

                if (activityTypes != null) {
                    this.AddRange(activityTypes);
                }
            }

            if (! this.Activities.Any() && File.Exists(seedsBasePath + "activities.json")) {
                var activities = JsonConvert.DeserializeObject<List<Activity>>(
                    File.ReadAllText(seedsBasePath + "activities.json")
                );

                if (activities != null) {
                    this.AddRange(activities);
                }
            }
            
            this.SaveChanges();
        }
    }
}