using System;
using Microsoft.AspNetCore.Identity;

namespace Runit.Backend.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
