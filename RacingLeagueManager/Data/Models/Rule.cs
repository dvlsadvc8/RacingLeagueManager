using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Rule
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }

        public Guid LeagueId { get; set; }
        public League League { get; set; }
    }
}
