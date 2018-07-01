using System;
using System.ComponentModel.DataAnnotations;

namespace Runit.Backend.Models
{
    public class Activity : ActivityBase
    {
        [Required]
        public DateTime Date { get; set; }
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }

        public UserPlanSubscription UserPlanSubscription {get;set;}
        public int? UserPlanSubscriptionId {get;set;}
    }
}
