using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Runit.Backend.Database;
using Runit.Backend.Models;

namespace Runit.Backend.Controllers
{
    [Route("api/[controller]")]
    public class ActivityController : Controller
    {
        private readonly RunitContext context;

        public ActivityController(RunitContext context) {
            this.context = context;
        }
        // GET api/activity
        [HttpGet]
        public IEnumerable<Activity> Get()
        {
            return context.Activities.ToList();
        }

        // GET api/activity/5
        [HttpGet("{id}")]
        public Activity Get(int id)
        {
            return context.Activities.FirstOrDefault(activity => activity.Id == id);
        }

        // POST api/activity
        [HttpPost]
        public void Post([FromBody] Activity value)
        {
        }

        // PUT api/activity/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Activity value)
        {
        }

        // DELETE api/activity/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
