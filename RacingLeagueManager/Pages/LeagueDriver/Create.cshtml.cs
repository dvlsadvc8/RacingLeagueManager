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

namespace RacingLeagueManager.Pages.LeagueDriver
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

        public async Task<IActionResult> OnGetAsync(Guid leagueId)
        {
            if(leagueId == null)
            {
                return NotFound();
            }

            League league = await _context.League.FirstOrDefaultAsync(m => m.Id == leagueId);

            if (league == null)
            {
                return NotFound();
            }

            LeagueDriver = new Data.Models.LeagueDriver() { LeagueId = leagueId, League = league  };
            
            return Page();
        }

        [BindProperty]
        public Data.Models.LeagueDriver LeagueDriver { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Driver driver = await _userManager.GetUserAsync(User);
            League league = await _context.League.FirstOrDefaultAsync(m => m.Id == LeagueDriver.LeagueId);

            if(driver == null || league == null)
            {
                return NotFound();
            }

            LeagueDriver.DriverId = driver.Id;
            
            await _context.LeagueDriver.AddAsync(LeagueDriver);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Leagues/Details", new { id = LeagueDriver.LeagueId });
        }
    }
}