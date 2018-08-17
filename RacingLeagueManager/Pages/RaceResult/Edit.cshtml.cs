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

namespace RacingLeagueManager.Pages.RaceResult
{
    public class EditModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public EditModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["DriverId"] = new SelectList(_context.Users, "Id", "Id");
           ViewData["RaceId"] = new SelectList(_context.Race, "Id", "Id");
           ViewData["SeriesEntryId"] = new SelectList(_context.SeriesEntry, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RaceResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceResultExists(RaceResult.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RaceResultExists(Guid id)
        {
            return _context.RaceResult.Any(e => e.Id == id);
        }
    }
}
