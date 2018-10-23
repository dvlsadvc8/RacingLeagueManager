using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesDriver
{
    public class DeleteModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DeleteModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SeriesDriver = await _context.SeriesDriver.FindAsync(id);

            if (SeriesDriver != null)
            {
                _context.SeriesDriver.Remove(SeriesDriver);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
