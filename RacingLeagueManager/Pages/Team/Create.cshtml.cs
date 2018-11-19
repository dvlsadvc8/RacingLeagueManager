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

        public async Task<IActionResult> OnGet(Guid seriesId, Guid? driverId)
        {
            if(seriesId == null)
            {
                return NotFound();
            }

            if(driverId == null)
            {
                driverId = ((Driver)(await _userManager.GetUserAsync(User))).Id;
            }

            if(_context.Team.Any(t => t.OwnerId == driverId && t.SeriesId == seriesId))
            {
                return BadRequest();
            }

            var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);
            if(series == null)
            {
                return NotFound();
            }

            Team = new Data.Models.Team() { SeriesId = series.Id, OwnerId = driverId.Value, Series = series };



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

            //Driver driver = await _userManager.GetUserAsync(User);
            //Team.OwnerId = driver.Id;

            _context.Team.Add(Team);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = Team.Id });
        }
    }
}