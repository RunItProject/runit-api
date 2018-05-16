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
    public class ActivityController : Controller
    {
        private readonly RunitContext context;
        private readonly UserManager<User> userManager;

        public ActivityController(RunitContext context, UserManager<User> userManager) {
            this.context = context;
            this.userManager = userManager;
        }
        // GET api/activity
        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<IEnumerable<Activity>> GetAsync()
        {
            return await context.Activities.ToListAsync();
        }

        // GET api/activity/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetForUserAsync(int userId)
        {
            var authenticatedUser = await GetAuthenticatedUserAsync();
            var forUser = await context.Users.FindAsync(userId);

            if (forUser == null) {
                return BadRequest();
            }

            if (! (forUser == authenticatedUser || User.IsInRole("Admin"))) {
                return Forbid();
            }

            var act = await context.Activities
                .Include(Activity => Activity.Type)
                .Where(activity => activity.User == forUser)
                .ToListAsync();

            return act;
        }


        // GET api/activity/5
        [HttpGet("{id}")]
        public async Task<Activity> GetAsync(int id)
        {
            return await context.Activities.FindAsync(id);
        }

        // POST api/activity
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Activity activity)
        {
            var authenticatedUser = await GetAuthenticatedUserAsync();

            if (! (activity.UserId == authenticatedUser.Id || User.IsInRole("Admin"))) { 
                return Forbid();
            }

            if (! ModelState.IsValid) {
                return BadRequest();
            }

            context.Activities.Add(activity);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAsync), new {id = activity.Id}, activity);
        }

        // PUT api/activity/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Activity updatedActivity)
        {
            var authenticatedUser = await GetAuthenticatedUserAsync();
            var oldActivity = await context.Activities.FindAsync(id);

            if (! ((oldActivity.UserId == authenticatedUser.Id && updatedActivity.UserId == authenticatedUser.Id) 
                    || User.IsInRole("Admin"))) { 
                return Forbid();
            }
            
            if (oldActivity == null) {
                return NotFound();
            }

            if (! ModelState.IsValid) {
                return BadRequest();
            }

            context.Entry(oldActivity).CurrentValues.SetValues(updatedActivity);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/activity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var authenticatedUser = await GetAuthenticatedUserAsync();
            var activity = await context.Activities.FindAsync(id);

            if (activity == null) {
                return NotFound();
            }

            if (! (activity.User == authenticatedUser || User.IsInRole("Admin"))) { 
                return Forbid();
            }

            context.Remove(activity);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<User> GetAuthenticatedUserAsync() 
        {
            return await userManager.GetUserAsync(User);
        }
    }
}
