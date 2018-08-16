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
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IList<Data.Models.Team> Team { get;set; }

        public async Task<IActionResult> OnGetAsync(Guid seriesId)
        {
            if(seriesId == null)
            {
                return NotFound();
            }

            var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);
            if(series == null)
            {
                return NotFound();
            }

            ViewData["SeriesId"] = series.Id;
            ViewData["SeriesName"] = series.Name;

            Team = await _context.Team
                .Include(t => t.Owner)
                .Include(t => t.Series).Where(t => t.SeriesId == seriesId).ToListAsync();

            return Page();
        }
    }
}
