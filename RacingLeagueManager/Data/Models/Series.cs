using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Series
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid LeagueId { get; set; }
        public League League { get; set; }

        public Guid OwnerId { get; set; }
        public Driver Owner { get; set; }

        public DateTime? StartDate { get; set; }

        public ICollection<SeriesEntry> SeriesEntries { get; set; }
        public ICollection<Race> Races { get; set; }

        public ICollection<Team> Teams { get; set; }

        public ICollection<SeriesDriver> SeriesDrivers { get; set; }
    }
}
