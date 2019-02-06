using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesEntry
{
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        //public IList<Data.Models.SeriesEntry> SeriesEntry { get;set; }
        public IList<StandingsViewModel> Standings { get; set; }
        public string SeriesName { get; set; }
        public Guid SeriesId { get; set; }


        public async Task OnGetAsync(Guid? seriesId)
        {
            if(seriesId == null)
            {
                NotFound();
            }

            var seriesEntries = await _context.SeriesEntry
                .Include(s => s.Car)
                .Include(s => s.Team)
                .Include(s => s.Series)
                .Include(s => s.RaceResults)
                .Include(s => s.SeriesEntryDrivers)
                    .ThenInclude(sed => sed.LeagueDriver)
                        .ThenInclude(ld => ld.Driver)
                .Where(se => se.SeriesId == seriesId)
                .ToListAsync();

            if(seriesEntries == null)
            {
                NotFound();
            }

            Standings = BuildModel(seriesEntries).ToList();
            SeriesName = seriesEntries[0].Series.Name;
            SeriesId = seriesEntries[0].SeriesId;
        }

        private IEnumerable<StandingsViewModel> BuildModel(IList<Data.Models.SeriesEntry> seriesEntryList)
        {
            List<StandingsViewModel> modelList = new List<StandingsViewModel>();

            modelList = seriesEntryList.Select(x =>
                new StandingsViewModel()
                {
                    Car = x.Car.Name,
                    Drivers = x.SeriesEntryDrivers.FirstOrDefault(sed => sed.DriverType == DriverType.Primary).LeagueDriver.Driver.DisplayUserName, //Where(sed => sed.DriverType == DriverType.Primary).FirstOrDefault().LeagueDriver.Driver.DisplayUserName,
                    RaceNumber = x.RaceNumber,
                    Team = x.Team.Name,
                    Points = x.RaceResults.Sum(r => r.Points)
                }
            ).ToList();

            var standings = modelList.OrderByDescending(x => x.Points).Select((x, i) =>
                new StandingsViewModel()
                {
                    Place = i + 1,
                    RaceNumber = x.RaceNumber,
                    Car = x.Car,
                    Drivers = x.Drivers,
                    Team = x.Team,
                    Points = x.Points
                }
            );

            return standings;
        }
    }



    public class StandingsViewModel
    {
        public int Place { get; set; }
        public string Drivers { get; set; }
        public string RaceNumber { get; set; }
        public string Team { get; set; }
        public string Car { get; set; }
        public int Points { get; set; }
    }
}
