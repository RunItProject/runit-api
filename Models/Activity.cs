using System;

namespace Runit.Backend.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public ActivityType Type { get; set; }
        public float Distance { get; set; }
        public User User { get; set; }
    }
}
