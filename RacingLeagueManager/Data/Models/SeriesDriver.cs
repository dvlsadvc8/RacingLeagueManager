using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class SeriesDriver
    {
        public Guid DriverId { get; set; }
        public Guid SeriesId { get; set; }
        public Guid LeagueId { get; set; }
        public string Status { get; set; }

        public LeagueDriver LeagueDriver { get; set; }
        public Series Series { get; set; }
        

    }
}
