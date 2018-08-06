using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult OnGet()
        {
        ViewData["RaceId"] = new SelectList(_context.Race, "Id", "Id");
        ViewData["SeriesId"] = new SelectList(_context.SeriesEntry, "SeriesId", "SeriesId");
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