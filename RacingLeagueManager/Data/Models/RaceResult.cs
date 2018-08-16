using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class RaceResult
    {
        public Guid Id { get; set; }

        public Guid RaceId { get; set; }
        public Race Race { get; set; }

        public Guid SeriesEntryId { get; set; }
        public SeriesEntry SeriesEntry { get; set; }

        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }

        public TimeSpan BestLap { get; set; }
        public TimeSpan TotalTime { get; set; }

        public int Place { get; set; }
        public int Points { get; set; }
    }
}
