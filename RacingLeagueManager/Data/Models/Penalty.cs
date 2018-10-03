using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Penalty
    {
        public Guid Id { get; set; }
        public Guid RaceResultId { get; set; }
        public RaceResult RaceResult { get; set; }

        public int Seconds { get; set; }

        public int LicensePoints { get; set; }

        public string Description { get; set; }
    }
}
 