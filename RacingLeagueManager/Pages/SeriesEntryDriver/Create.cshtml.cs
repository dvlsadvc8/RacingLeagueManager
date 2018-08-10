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

namespace RacingLeagueManager.Pages.SeriesEntryDriver
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(Guid seriesEntryId)
        {
            var seriesEntry = await _context.SeriesEntry.Include(s => s.Series).FirstOrDefaultAsync(s => s.Id == seriesEntryId);
            if(seriesEntry == null)
            {
                return NotFound();
            }

            ViewData["DriverId"] = new SelectList(await _context.LeagueDriver.Include(l => l.Driver).Where(l => l.LeagueId == seriesEntry.Series.LeagueId).ToListAsync(), "DriverId", "Driver.UserName");            

            SeriesEntryDriver = new Data.Models.SeriesEntryDriver() { SeriesEntryId = seriesEntry.Id, LeagueId = seriesEntry.Series.LeagueId };

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