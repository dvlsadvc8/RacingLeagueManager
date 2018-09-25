using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Leagues
{
    public class DetailsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;
        private readonly UserManager<Driver> _userManager;

        public DetailsModel(UserManager<Driver> userManager, RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public League League { get; set; }
        public bool IsJoinable { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            League = await _context.League
                .Include(l => l.Owner)
                .Include(l => l.Series)
                    .ThenInclude(s => s.Owner)
                .Include(l => l.LeagueDrivers)
                    .ThenInclude(ld => ld.Driver)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (League == null)
            {
                return NotFound();
            }

            Driver driver = await _userManager.GetUserAsync(User);
            IsJoinable = !League.LeagueDrivers.Any(ld => ld.DriverId == driver.Id);

            return Page();
        }

    }
}
