using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Runit.Backend.Database.Seeds;
using Runit.Backend.Models;

namespace Runit.Backend.Database
{
    public class RunitContext : IdentityDbContext<User, UserRole, int>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<UserFeed> UserFeeds { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanActivity> PlanActivities { get; set; }
        public RunitContext(DbContextOptions<RunitContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Activity>()
                .HasOne(activity => activity.Type)
                .WithMany()
                .HasForeignKey(activity => activity.TypeId)
                .IsRequired();
        }
    }
}