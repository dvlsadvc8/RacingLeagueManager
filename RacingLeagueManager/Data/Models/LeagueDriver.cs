using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class LeagueDriver
    {
        public Guid LeagueId { get; set; }
        public Guid DriverId { get; set; }
        
        public Driver Driver { get; set; }
        public League League { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss\\.fff}", ApplyFormatInEditMode = true)] 
        public TimeSpan PreQualifiedTime { get; set; }
        public int? TrueSkillRating { get; set; }

        public string Status { get; set; }

        public ICollection<SeriesEntryDriver> SeriesEntryDrivers { get; set; }
        public ICollection<SeriesDriver> SeriesDrivers { get; set; }
    }
}
