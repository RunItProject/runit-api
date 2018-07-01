using System;
using System.Collections.Generic;

namespace Runit.Backend.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PlanActivity> Activities { get; set; }

        public List<UserPlanSubscription> UserPlanSubscriptions { get; set; }
    }
}
