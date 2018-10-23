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

namespace RacingLeagueManager.Pages.SeriesDriver
{
    public class EditModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public EditModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Models.SeriesDriver SeriesDriver { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SeriesDriver = await _context.SeriesDriver
                .Include(s => s.LeagueDriver)
                .Include(s => s.Series).FirstOrDefaultAsync(m => m.DriverId == id);

            if (SeriesDriver == null)
            {
                return NotFound();
            }
           ViewData["LeagueId"] = new SelectList(_context.LeagueDriver, "LeagueId", "LeagueId");
           ViewData["SeriesId"] = new SelectList(_context.Series, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SeriesDriver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeriesDriverExists(SeriesDriver.DriverId))
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

        private bool SeriesDriverExists(Guid id)
        {
            return _context.SeriesDriver.Any(e => e.DriverId == id);
        }
    }
}
