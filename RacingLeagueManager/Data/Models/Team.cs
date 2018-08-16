using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid SeriesId { get; set; }
        public Series Series { get; set; }

        public Guid OwnerId { get; set; }
        public Driver Owner { get; set; }

        public ICollection<SeriesEntry> SeriesEntries { get; set; }
        
    }
}
