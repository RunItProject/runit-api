using System;
using System.ComponentModel.DataAnnotations;

namespace Runit.Backend.Models
{
    public class UserFeed
    {
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }
        public string URL { get; set; }
        public string Token { get; set; }
    }
}
