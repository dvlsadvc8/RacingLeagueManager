using Microsoft.AspNetCore.Identity;
using Moserware.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Driver : IdentityUser<Guid>
    {
        public string DisplayUserName { get; set; }

        public ICollection<LeagueDriver> LeagueDrivers { get; set; }

        public ICollection<Series> OwnedSeries { get; set; }
        public Guid OwnedLeagueId { get; set; }
        public League OwnedLeague { get; set; }

        public double TrueSkillMean { get; set; }
        public double TrueSkillStandardDeviation { get; set; }
        public double TrueSkillConservativeRating { get; set; }

        public Rating Rating
        {
            get
            {
                return new Rating(TrueSkillMean, TrueSkillStandardDeviation);
            }
        }

        public ICollection<RaceResult> RaceResults { get; set; }

        public ICollection<SeriesEntryDriver> SeriesEntryDrivers { get; set; }

        public Driver() : base()
        {

        }
    }
}
