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
    public class DetailsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DetailsModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public Data.Models.SeriesEntryDriver SeriesEntryDriver { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SeriesEntryDriver = await _context.SeriesEntryDriver
                .Include(s => s.LeagueDriver)
                .Include(s => s.SeriesEntry).FirstOrDefaultAsync(m => m.LeagueId == id);

            if (SeriesEntryDriver == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
