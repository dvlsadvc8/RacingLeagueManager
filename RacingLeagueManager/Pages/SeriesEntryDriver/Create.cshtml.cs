using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesEntryDriver
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid seriesEntryId)
        {
        ViewData["LeagueId"] = new SelectList(_context.LeagueDriver, "LeagueId", "LeagueId");
            //ViewData["SeriesEntryId"] = new SelectList(_context.SeriesEntry, "Id", "Id");

            SeriesEntryDriver = new Data.Models.SeriesEntryDriver() { SeriesEntryId = seriesEntryId };

            return Page();
        }

        [BindProperty]
        public Data.Models.SeriesEntryDriver SeriesEntryDriver { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SeriesEntryDriver.Add(SeriesEntryDriver);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}