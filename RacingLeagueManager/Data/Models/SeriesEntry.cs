using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class SeriesEntry
    {
        public Guid Id { get; set; }
        public ICollection<SeriesEntryDriver> SeriesEntryDrivers { get; set; }
        public Guid SeriesId { get; set; }
        public Series Series { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public Car Car { get; set; }
        public Guid CarId { get; set; }

        public string RaceNumber { get; set; }

        public ICollection<RaceResult> RaceResults { get; set; }


    }
}
