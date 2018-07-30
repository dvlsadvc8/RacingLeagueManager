using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Race> Races { get; set; }

    }
}
