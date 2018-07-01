using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Runit.Backend.Models;

namespace Runit.Backend.Database.Seeds
{
    public abstract class Seeder
    {
        protected readonly RunitContext context;

        public Seeder(RunitContext context) {
            this.context = context;
        }
        public abstract Task RunAsync();
        public abstract bool ShouldRun();
    }
}