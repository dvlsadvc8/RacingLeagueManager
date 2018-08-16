using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesEntry
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid seriesId, Guid teamId)
        {
            if(seriesId == null || teamId == null)
            {
                return NotFound();
            }

            var series = _context.Series.FirstOrDefault(s => s.Id == seriesId);
            var team = _context.Team.FirstOrDefault(t => t.Id == teamId);

            if(series == null || team == null)
            {
                return NotFound();
            }

            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Name");

            SeriesEntry = new Data.Models.SeriesEntry() { SeriesId = seriesId, Series = series, TeamId = teamId, Team = team };

            return Page();
        }

        [BindProperty]
        public Data.Models.SeriesEntry SeriesEntry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SeriesEntry.Add(SeriesEntry);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Team/Details", new { id = SeriesEntry.TeamId });
        }
    }
}