using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Race
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid seriesId)
        {
            Data.Models.Series series = _context.Series.FirstOrDefault(s => s.Id == seriesId);
            if(series == null)
            {
                return NotFound();
            }

            ViewData["TrackId"] = new SelectList(_context.Track, "Id", "Name");
            Race = new Data.Models.Race() { SeriesId = seriesId, RaceDate = DateTime.Today, Series=series };

            return Page();
        }

        [BindProperty]
        public Data.Models.Race Race { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Race.Status = RaceStatus.Pending;

            _context.Race.Add(Race);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Series/Details", new { id = Race.SeriesId });
        }
    }
}