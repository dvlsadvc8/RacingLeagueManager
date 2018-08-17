using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Internal;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Race
{
    public class DetailsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DetailsModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public Data.Models.Race Race { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Race = await _context.Race
                .Include(r => r.Results)
                .Include(r => r.Track)
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(s => s.SeriesEntryDrivers)
                            .ThenInclude(s => s.LeagueDriver)
                                .ThenInclude(ld => ld.Driver)
                
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(e => e.RaceResults)
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(s => s.Car)
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(s => s.Team)
                .FirstOrDefaultAsync(m => m.Id == id);

            //.ThenInclude(s => s.Entries.Select(e => e.!_context.RaceResult.Any(r => r.SeriesId == e.SeriesId && r.LeagueId == e.LeagueId && r.DriverId == e.DriverId)));

            //var entriesWithoutResult = _context.SeriesEntry.Include(se => se.SeriesId == Race.SeriesId)



            //Race.Series.Entries = entriesWithoutResult;

                //_context.SeriesEntry.Where(se => se.SeriesId == Race.SeriesId && se.Results.Any(se.Results.Where(r => r.Race.Id == Race.Id)));

            if (Race == null)
            {
                return NotFound();
            }
            return Page();
        }

        //public class RaceDetailViewModel
        //{
        //    public Guid RaceId { get; set; }
        //    public string TrackName { get; set; }
        //    public Guid SeriesId { get; set; }
        //    public string SeriesName { get; set; }
        //    public int Laps { get; set; }
        //    public DateTime RaceDate { get; set; }

        //    public List<RaceResultViewModel> Results { get; set; }
        //}

        //public class RaceResultViewModel
        //{
        //    public Guid RaceId { get; set; }
        //    public Guid SeriesEntryId { get; set; }

            
        //}
    }
}
