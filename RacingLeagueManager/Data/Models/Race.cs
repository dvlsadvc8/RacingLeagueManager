using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class Race
    {
        public Guid Id { get; set; }
        public Guid TrackId { get; set; }
        public Track Track { get; set; }

        public RaceStatus? Status { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RaceDate { get; set; }
        public int Laps { get; set; }
        public ICollection<RaceResult> Results { get; set; }

        public Guid SeriesId { get; set; }
        public Series Series { get; set; }
    }

    public enum RaceStatus
    {
        Pending = 0,
        Open = 1,
        Closed = 2,
        Certified = 3
    }
}
