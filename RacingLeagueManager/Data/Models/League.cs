using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RacingLeagueManager.Data.Models
{
    public class League
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid OwnerId { get; set; }
        public Driver Owner { get; set; }

        public ICollection<Series> Series { get; set; }
        //public ICollection<Driver> Drivers { get; set; }
        public ICollection<LeagueDriver> LeagueDrivers { get; set; }
        
    }
}
