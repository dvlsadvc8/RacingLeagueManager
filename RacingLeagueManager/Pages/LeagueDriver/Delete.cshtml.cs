using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.LeagueDriver
{
    public class DeleteModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DeleteModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Models.LeagueDriver LeagueDriver { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LeagueDriver = await _context.LeagueDriver
                .Include(l => l.Driver)
                .Include(l => l.League).FirstOrDefaultAsync(m => m.LeagueId == id);

            if (LeagueDriver == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LeagueDriver = await _context.LeagueDriver.FindAsync(id);

            if (LeagueDriver != null)
            {
                _context.LeagueDriver.Remove(LeagueDriver);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
