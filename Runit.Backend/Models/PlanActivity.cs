using System;
using System.Collections.Generic;

namespace Runit.Backend.Models
{
    public class PlanActivity : ActivityBase
    {
        public int PlanId { get; set; }
        public Plan Plan { get; set; }
        public int Week { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
