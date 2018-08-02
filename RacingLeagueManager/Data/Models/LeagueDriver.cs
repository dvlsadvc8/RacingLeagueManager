using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class LeagueDriver
    {
        public Guid LeagueId { get; set; }
        public Guid DriverId { get; set; }
        

        public Driver Driver { get; set; }
        public League League { get; set; }

        public int RaceNumber { get; set; }

        public ICollection<SeriesEntry> SeriesEntries { get; set; }
    }
}
