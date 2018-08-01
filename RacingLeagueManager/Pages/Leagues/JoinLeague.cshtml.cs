using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Leagues
{
    public class JoinLeagueModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;
        private readonly UserManager<Driver> _userManager;

        public JoinLeagueModel(UserManager<Driver> userManager, RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [BindProperty]
        public string LeagueName { get; set; }
        [BindProperty]
        public Guid LeagueId { get; set; }
        //[BindProperty]
        //public string DriverName { get; set; }
        //[BindProperty]
        //public Guid DriverId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid leagueId)
        {
            if(leagueId == null)
            {
                return NotFound();
            }

            //Driver driver = await _userManager.GetUserAsync(User);
            League league = await _context.League.FirstOrDefaultAsync(m => m.Id == leagueId);

            if(league == null)
            {
                return NotFound();
            }

            LeagueName = league.Name;
            LeagueId = league.Id;
            //DriverName = driver.UserName;
            //DriverId = driver.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            Driver driver = await _userManager.GetUserAsync(User);
            League league = await _context.League.FirstOrDefaultAsync(m => m.Id == LeagueId);

            Data.Models.LeagueDriver leagueDriver = new LeagueDriver { LeagueId = league.Id, DriverId = driver.Id };

            //await _userManager.UpdateAsync(driver);
            await _context.AddAsync(leagueDriver);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { Id = LeagueId });
        }

        
    }
}