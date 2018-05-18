using System;
using System.ComponentModel.DataAnnotations;

namespace Runit.Backend.Models
{
    public abstract class ActivityBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ActivityType Type { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required]
        [Range(0.0, float.PositiveInfinity)]
        public double Distance { get; set; }
    }
}
