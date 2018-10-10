using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesEntryDriver
{
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        //public IList<Data.Models.SeriesEntryDriver> SeriesEntryDriver { get;set; }
        public IList<SeriesEntryDriverViewModel> Drivers { get; set; }

        public async Task OnGetAsync(Guid seriesId)
        {
            int raceCount = 10;

            //var raceResults = await _context.RaceResult.Include(r => r.Race).Where(r => r.Race.SeriesId == seriesId).ToListAsync();

            var query = _context.SeriesEntryDriver
                .Include(s => s.LeagueDriver)
                    .ThenInclude(ld => ld.Driver)
                        .ThenInclude(d => d.RaceResults)
                .Include(s => s.SeriesEntry)
                .Where(s => s.SeriesEntry.SeriesId == seriesId)

                .Select(s => new SeriesEntryDriverViewModel
                {
                    DriverId = s.DriverId,
                    DriverName = s.LeagueDriver.Driver.UserName,
                    DriverType = s.DriverType,
                    DNFCount = s.LeagueDriver.Driver.RaceResults.Where(r => r.ResultType == ResultType.DNF).Count()
                    //DNFPercent = (s.LeagueDriver.Driver.RaceResults.Where(r => r.ResultType == ResultType.DNF).Count() / (decimal)raceCount) * 100,
                    //DNSPercent = (s.LeagueDriver.Driver.RaceResults.Where(r => r.ResultType == ResultType.DNS).Count() / (decimal)raceCount) * 100

                });

            Drivers = await query.ToListAsync();
        }
    }

    public class SeriesEntryDriverViewModel
    {
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public DriverType DriverType { get; set; }
        //public int RaceCount { get; set; }
        public int DNFCount { get; set; }
        public decimal DNFPercent
        {
            get
            {
                return (DNFCount / (decimal)10.00) * 100;
            }
        }
        //public int DNSCount { get; set; }
        public decimal DNSPercent { get; set; }

    }
}
