using Microsoft.EntityFrameworkCore;

namespace Runit.Backend.Models
{
    public class RunitContext : DbContext
    {
        public RunitContext(DbContextOptions<RunitContext> options)
            : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }

    }
}