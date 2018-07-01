using System;
using Microsoft.AspNetCore.Identity;

namespace Curnik.Models.IdentityModels
{
    public class AppUser : IdentityUser<int>
    {
        public string AccountDescription { get; set; }
        public DateTime LastSeenOnline { get; set; }
        public DateTime AccountCreateTime { get; set; }
    }
}
