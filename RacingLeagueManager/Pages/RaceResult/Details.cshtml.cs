using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.RaceResult
{
    public class DetailsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DetailsModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public Data.Models.RaceResult RaceResult { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RaceResult = await _context.RaceResult
                .Include(r => r.Driver)
                .Include(r => r.Race)
                .Include(r => r.SeriesEntry).FirstOrDefaultAsync(m => m.Id == id);

            if (RaceResult == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
