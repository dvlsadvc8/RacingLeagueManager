using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class SeriesEntry
    {
        public Guid DriverId { get; set; }
        public Guid LeagueId { get; set; }
        public LeagueDriver LeagueDriver { get; set; }
        public Guid SeriesId { get; set; }
        public Series Series { get; set; }
        public Car Car { get; set; }
        public Guid CarId { get; set; }

        public ICollection<RaceResult> Results { get; set; }


    }
}
