using System;
using System.ComponentModel.DataAnnotations;

namespace Runit.Backend.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public ActivityType Type { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required]
        [Range(0.0, float.PositiveInfinity)]
        public double Distance { get; set; }
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
