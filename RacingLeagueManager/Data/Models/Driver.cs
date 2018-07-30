using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Driver : IdentityUser<Guid>
    {
        public ICollection<LeagueDriver> LeagueDrivers { get; set; }

        public Driver() : base()
        {

        }
    }
}
