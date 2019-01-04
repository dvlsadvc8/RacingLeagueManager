using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Driver")]
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss\\.fff}", ApplyFormatInEditMode = true)]
        public TimeSpan? BestLap { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan? TotalTime { get; set; }

        public int Place { get; set; }
        public int Points { get; set; }
        public int PenaltyPoints { get; set; }

        public ResultType? ResultType { get; set; }
        

        public virtual ICollection<Penalty> Penalties { get; set; }
    }

    public enum ResultType
    {
        Finished,
        DNF,
        DNS,
        RC,
        DQ
    }
}
