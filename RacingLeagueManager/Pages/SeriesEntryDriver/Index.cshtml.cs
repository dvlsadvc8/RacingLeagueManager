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
        public Guid SeriesId { get; set; }
        public string SeriesName {get; set; }

        public async Task OnGetAsync(Guid seriesId)
        {
            var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);
            SeriesId = series.Id;
            SeriesName = series.Name;
            
            var query = _context.SeriesEntryDriver
                .Include(s => s.LeagueDriver)
                    .ThenInclude(ld => ld.Driver)
                        .ThenInclude(d => d.RaceResults)
                            .ThenInclude(rr => rr.SeriesEntry)

                .Include(s => s.LeagueDriver)
                    .ThenInclude(ld => ld.Driver)
                        .ThenInclude(d => d.RaceResults)
                            .ThenInclude(r => r.Penalties)

                .Include(s => s.SeriesEntry)
                .Where(s => s.SeriesEntry.SeriesId == seriesId && s.DriverType != DriverType.Retired)
                
                .Select(s => new SeriesEntryDriverViewModel
                {
                    DriverId = s.DriverId,
                    DisplayUserName = s.LeagueDriver.Driver.DisplayUserName,
                    DriverType = s.DriverType,
                    RaceResultCount = s.LeagueDriver.Driver.RaceResults.Where(rr => rr.SeriesEntry.SeriesId == seriesId).Count(),
                    DNFCount = s.LeagueDriver.Driver.RaceResults.Where(r => r.ResultType == ResultType.DNF && r.SeriesEntry.SeriesId == seriesId).Count(),
                    DNSCount = s.LeagueDriver.Driver.RaceResults.Where(r => r.ResultType == ResultType.DNS && r.SeriesEntry.SeriesId == seriesId).Count(),
                    PenaltyPoints = s.LeagueDriver.Driver.RaceResults.Where(r => r.SeriesEntry.SeriesId == seriesId && r.Penalties.Count() > 0).Count() //Sum(r => r.Penalties.Count() > 1 ? 1 : 0)
                });

            Drivers = await query.Distinct().ToListAsync();

            
        }
    }

    public class SeriesEntryDriverViewModel
    {
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public string DisplayUserName { get; set; }
        public DriverType DriverType { get; set; }
        public int RaceResultCount { get; set; }
        public int PenaltyPoints { get; set; }
        //public int RaceCount { get; set; }
        public int DNFCount { get; set; }
        public int DNSCount { get; set; }
        public decimal DNFPercent
        {
            get
            {
                if(RaceResultCount > 0)
                {
                    return (DNFCount / (decimal)RaceResultCount) * 100;
                }
                else
                {
                    return 0;
                }
                
            }
        }

        public decimal DNSPercent
        {
            get
            {
                if (RaceResultCount > 0)
                {
                    return (DNSCount / (decimal)RaceResultCount) * 100;
                }
                else
                {
                    return 0;
                }
            }
        }
        public decimal PenaltyPointPercent
        {
            get
            {
                if (RaceResultCount > 0)
                {
                    return (PenaltyPoints / (decimal)RaceResultCount) * 100;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
