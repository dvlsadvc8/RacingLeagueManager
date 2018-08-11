using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Team
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

        public async Task<IActionResult> OnGet(Guid seriesId)
        {
            var Series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);
            if(Series == null)
            {
                return NotFound();
            }

            Team = new Data.Models.Team() { SeriesId = Series.Id };

            return Page();
        }

        [BindProperty]
        public Data.Models.Team Team { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Driver driver = await _userManager.GetUserAsync(User);
            Team.OwnerId = driver.Id;

            _context.Team.Add(Team);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { seriesId = Team.SeriesId });
        }
    }
}