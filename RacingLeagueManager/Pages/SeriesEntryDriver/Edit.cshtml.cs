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
    public class EditModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public EditModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["LeagueId"] = new SelectList(_context.LeagueDriver, "LeagueId", "LeagueId");
           ViewData["SeriesEntryId"] = new SelectList(_context.SeriesEntry, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SeriesEntryDriver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeriesEntryDriverExists(SeriesEntryDriver.LeagueId))
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

        private bool SeriesEntryDriverExists(Guid id)
        {
            return _context.SeriesEntryDriver.Any(e => e.LeagueId == id);
        }
    }
}
