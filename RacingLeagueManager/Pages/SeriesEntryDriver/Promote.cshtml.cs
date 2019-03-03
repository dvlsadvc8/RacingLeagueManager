using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Authorization;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;
using RacingLeagueManager.Pages.Shared;

namespace RacingLeagueManager.Pages.SeriesEntryDriver
{
    public class PromoteModel : DI_BasePageModel
    {
        public PromoteModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Data.Models.SeriesEntryDriver SeriesEntryDriver { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? leagueId, Guid? seriesEntryId, Guid? driverId)
        {
            if (leagueId == null || seriesEntryId == null || driverId == null)
            {
                return BadRequest();
            }

            SeriesEntryDriver = await _context.SeriesEntryDriver
                .Include(s => s.LeagueDriver)
                    .ThenInclude(l => l.Driver)
                .Include(s => s.SeriesEntry).FirstOrDefaultAsync(m => m.LeagueId == leagueId && m.SeriesEntryId == seriesEntryId && m.DriverId == driverId);

            if (SeriesEntryDriver == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? leagueId, Guid? seriesEntryId, Guid? driverId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var driver = await _context.SeriesEntryDriver
                .Include(s => s.SeriesEntry)
                .FirstOrDefaultAsync(m => m.LeagueId == leagueId && m.SeriesEntryId == seriesEntryId && m.DriverId == driverId);

            if(driver == null)
            {
                return NotFound();
            }

            var team = await _context.Team.FirstOrDefaultAsync(t => t.Id == driver.SeriesEntry.TeamId);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, team,
                                                Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            var seriesDriver = await _context.SeriesDriver.FirstOrDefaultAsync(s => s.DriverId == driverId && s.SeriesId == driver.SeriesEntry.SeriesId);

            driver.DriverType = DriverType.Primary;
            seriesDriver.Status = "Unavailable - Primary";

            await _context.SaveChangesAsync();

            return RedirectToPage("../Team/Details", new { id = team.Id });
        }
    }
}
