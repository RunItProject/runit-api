using System;
using System.ComponentModel.DataAnnotations;

namespace Runit.Backend.Models
{
    public class UserPlanSubscription
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Plan Plan { get; set; }
        public int PlanId { get; set; }
    }
}
