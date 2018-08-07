using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.RaceResult
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid raceId, Guid driverId)
        {
            //ViewData["RaceId"] = new SelectList(_context.Race, "Id", "Id");
            //ViewData["SeriesId"] = new SelectList(_context.SeriesEntry, "SeriesId", "SeriesId");

            var race = await _context.Race.Include(r => r.Track).Include(r => r.Series).ThenInclude(s => s.Entries.Where(e => e.DriverId == driverId)).ThenInclude(e => e.LeagueDriver).ThenInclude(ld => ld.Driver).FirstOrDefaultAsync(r => r.Id == raceId);
            if(race == null)
            {
                return NotFound();
            }

            RaceResult = new Data.Models.RaceResult() { RaceId = race.Id, SeriesId = race.SeriesId, DriverId = driverId, LeagueId = race.Series.LeagueId  };

            return Page();
        }

        [BindProperty]
        public Data.Models.RaceResult RaceResult { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RaceResult.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}