using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Team
{
    public class DetailsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DetailsModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public Data.Models.Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Team
                .Include(t => t.Owner)
                .Include(t => t.Series)
                .Include(t => t.SeriesEntries)
                    .ThenInclude(s => s.Car)
                .Include(s => s.SeriesEntries)
                .ThenInclude(s => s.SeriesEntryDrivers)
                    .ThenInclude(s => s.LeagueDriver)
                        .ThenInclude(ld => ld.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Team == null)
            {
                return NotFound();
            }

            foreach(var entry in Team.SeriesEntries)
            {
                entry.SeriesEntryDrivers = entry.SeriesEntryDrivers.OrderBy(s => s.DriverType).ToList();
            }

            return Page();
        }
    }
}
