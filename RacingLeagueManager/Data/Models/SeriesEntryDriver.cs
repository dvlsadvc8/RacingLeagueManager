using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class SeriesEntryDriver
    {
        public Guid LeagueId { get; set; }
        public Guid DriverId { get; set; }
        public LeagueDriver LeagueDriver { get; set; }

        public Guid SeriesEntryId { get; set; }
        public SeriesEntry SeriesEntry { get; set; }

        public DriverType DriverType { get; set; }
    }

    public enum DriverType
    {
        Primary,
        Reserve,
        Retired
    }
}
