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

namespace RacingLeagueManager.Pages.LeagueDriver
{
    public class EditModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public EditModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Models.LeagueDriver LeagueDriver { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid leagueId, Guid driverId)
        {
            if (leagueId == null || driverId == null)
            {
                return NotFound();
            }

            LeagueDriver = await _context.LeagueDriver
                .Include(l => l.Driver)
                .Include(l => l.League).FirstOrDefaultAsync(m => m.LeagueId == leagueId && m.DriverId == driverId);

            if (LeagueDriver == null)
            {
                return NotFound();
            }
           ViewData["DriverId"] = new SelectList(_context.Users, "Id", "Id");
           ViewData["LeagueId"] = new SelectList(_context.League, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LeagueDriver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueDriverExists(LeagueDriver.LeagueId))
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

        private bool LeagueDriverExists(Guid id)
        {
            return _context.LeagueDriver.Any(e => e.LeagueId == id);
        }
    }
}
