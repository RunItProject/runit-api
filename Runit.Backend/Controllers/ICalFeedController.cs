using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Runit.Backend.Database;
using Runit.Backend.Models;

namespace Runit.Backend.Controllers
{
    [Authorize]
    [Route("api/feed/ical")]
    public class ICalFeedController : Controller
    {
        private readonly RunitContext context;
        private readonly UserManager<User> userManager;

        public ICalFeedController(RunitContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        // GET api/feed/ical/{token}
        [HttpGet("{token}.ics")]
        [Produces("text/calendar")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Activity>>> Feed(string token)
        {
            UserFeed feed = await context.UserFeeds.SingleAsync(f => f.Token == token);

            var activites = await context.Activities
                .Where(activity => activity.UserId == feed.UserId)
                .ToListAsync();
            
            var res = new OkObjectResult(activites);
            
            return activites;
        }

        // GET api/feed/ical/
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFeed>>> GetAsync()
        {
            return await context.UserFeeds.ToListAsync();
        }

        // GET api/feed/ical/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFeed>> GetAsync(int id)
        {
            var authenticatedUser = await userManager.GetUserAsync(User);
            var feed = await context.UserFeeds.FindAsync(id);

            if (feed == null)
            {
                return NotFound();
            }

            if (!(feed.UserId == authenticatedUser.Id || User.IsInRole("Admin")))
            {
                return Forbid();
            }

            return feed;
        }

        // GET api/feed/ical/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserFeed>>> GetForUserAsync(int userId)
        {
            var authenticatedUser = await userManager.GetUserAsync(User);

            if (!(userId == authenticatedUser.Id || User.IsInRole("Admin")))
            {
                return Forbid();
            }

            return await context.UserFeeds.Where(feed => feed.UserId == userId).ToListAsync();
        }

        // POST api/feed/ical
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CreateFeedDto createFeedDto)
        {
            var authenticatedUser = await userManager.GetUserAsync(User);

            if (!(createFeedDto.UserId == authenticatedUser.Id || User.IsInRole("Admin")))
            {
                return Forbid();
            }

            var token = Guid.NewGuid().ToString("N");
            var feed = new UserFeed()
            {
                Created = DateTime.Now,
                UserId = createFeedDto.UserId,
                URL = Url.Action(nameof(Feed), new { Token = token }),
                Token = token
            };

            context.Add(feed);
            await context.SaveChangesAsync();

            return CreatedAtAction(feed.URL, new { Token = feed.Token }, feed);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var authenticatedUser = await userManager.GetUserAsync(User);
            var feed = await context.UserFeeds.FindAsync(id);

            if (feed == null)
            {
                return NotFound();
            }

            if (!(feed.UserId == authenticatedUser.Id || User.IsInRole("Admin")))
            {
                return Forbid();
            }

            context.Remove(feed);
            await context.SaveChangesAsync();

            return NoContent();
        }

        public class CreateFeedDto
        {
            public int UserId { get; set; }
        }
    }
}
