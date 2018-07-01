using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Runit.Backend.Database;
using Runit.Backend.Models;

namespace Runit.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PlanController : Controller
    {
        private readonly RunitContext context;
        private readonly UserManager<User> userManager;

        public PlanController(RunitContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        // GET api/plan
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<Plan>> GetAsync()
        {
            throw new NotImplementedException();
        }

        // POST api/plan/{planId}/user/{userId}
        [HttpGet("{planId}/user/{userId}")]
        public async Task<ActionResult> Start(int planId, int userId)
        {
            var authenticatedUser = await userManager.GetUserAsync(User);
            var forUser = await context.Users.FindAsync(userId);
            var plan = await context.Plans.Include(p => p.Activities).SingleOrDefaultAsync(p => p.Id == planId);

            if (forUser == null || plan == null)
            {
                return BadRequest();
            }

            if (!(forUser == authenticatedUser || User.IsInRole("Admin")))
            {
                return Forbid();
            }

            var subscription = new UserPlanSubscription() {
                StartedAt = DateTime.Now,
                User = forUser,
                Plan = plan
            };

            var newActivities = plan.Activities.Select<PlanActivity, Activity>(planActivity => new Activity()
            {
                Title = planActivity.Title,
                TypeId = planActivity.TypeId, 
                Distance = planActivity.Distance,
                Date = DateTime.Now.AddDays(7*planActivity.Week).AddDays((int) planActivity.DayOfWeek),
                User = forUser,
                UserPlanSubscription = subscription
            });

            context.AddRange(newActivities);
            await context.SaveChangesAsync();

            return NoContent();
        }


        // GET api/plan/5
        [HttpGet("{id}")]
        public async Task<Plan> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/plan
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Plan Plan)
        {
            throw new NotImplementedException();
        }

        // PUT api/plan/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Plan updatedPlan)
        {
            throw new NotImplementedException();
        }

        // DELETE api/plan/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
