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

namespace RacingLeagueManager.Pages.Penalty
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid raceResultId)
        {
            RaceResult = await _context.RaceResult
                .Include(r => r.Race)
                    .ThenInclude(r => r.Track)
                .Include(r => r.Driver)
                .Include(r => r.SeriesEntry)
                    .ThenInclude(s => s.Series)
                .FirstOrDefaultAsync(r => r.Id == raceResultId);

            if(RaceResult == null)
            {
                return NotFound();
            }

            Penalty = new Data.Models.Penalty() { RaceResultId = raceResultId };

            return Page();
        }

        [BindProperty]
        public Data.Models.Penalty Penalty { get; set; }

        public Data.Models.RaceResult RaceResult { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Penalty.Add(Penalty);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}