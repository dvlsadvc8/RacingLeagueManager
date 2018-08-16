using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Series
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;
        private readonly UserManager<Driver> _userManager;

        public CreateModel(UserManager<Driver> userManager, RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult OnGet(Guid leagueId)
        {
            //ViewData["LeagueId"] = new SelectList(_context.League, "Id", "Id");
            League league = _context.League.FirstOrDefault(l => l.Id == leagueId);
            if(league == null)
            {
                return NotFound();
            }

            Series = new Data.Models.Series() { LeagueId = leagueId };

            return Page();
        }

        [BindProperty]
        public Data.Models.Series Series { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Driver driver = await _userManager.GetUserAsync(User);
            Series.OwnerId = driver.Id;

            _context.Series.Add(Series);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = Series.Id });
        }
    }
}