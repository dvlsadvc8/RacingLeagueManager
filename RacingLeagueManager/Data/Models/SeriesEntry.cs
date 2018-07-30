using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RacingLeagueManager.Data.Models
{
    public class SeriesEntry
    {
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }
        public Guid SeriesId { get; set; }
        public Series Series { get; set; }
        public Car Car { get; set; }
        public Guid CarId { get; set; }

        
    }
}
