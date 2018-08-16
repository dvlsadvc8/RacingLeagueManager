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

        public ICollection<Series> OwnedSeries { get; set; }
        public Guid OwnedLeagueId { get; set; }
        public League OwnedLeague { get; set; }

        public Driver() : base()
        {

        }
    }
}
