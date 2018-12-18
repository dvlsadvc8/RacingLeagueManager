using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Authorization;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;
using RacingLeagueManager.Pages.Shared;

namespace RacingLeagueManager.Pages.RaceResult
{
    public class EditModel : DI_BasePageModel
    {

        public EditModel(RacingLeagueManager.Data.RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Data.Models.RaceResult RaceResult { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RaceResult = await _context.RaceResult
                .Include(r => r.Driver)
                .Include(r => r.Race)
                .Include(r => r.SeriesEntry).FirstOrDefaultAsync(m => m.Id == id);

            if (RaceResult == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, RaceResult,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            var teamDrivers = await _context.SeriesEntryDriver
                .Include(sed => sed.Driver)
                .Where(sed => sed.SeriesEntryId == RaceResult.SeriesEntryId)
                .Select(sed => new { Id = sed.Driver.Id, DisplayUserName = string.Format("{0}-{1}", sed.Driver.DisplayUserName, sed.DriverType) })
                .ToListAsync();

            ViewData["DriverId"] = new SelectList(teamDrivers, "Id", "DisplayUserName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var raceResult = await _context.RaceResult.Include(r => r.s)//.Attach(RaceResult).State = EntityState.Modified;

            var raceResult = await _context.RaceResult
                .Include(r => r.Driver)
                .Include(r => r.Race)
                .Include(r => r.SeriesEntry).FirstOrDefaultAsync(m => m.Id == RaceResult.Id);

            if(raceResult == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, raceResult,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            raceResult.DriverId = RaceResult.DriverId;
            raceResult.BestLap = RaceResult.BestLap;
            raceResult.TotalTime = RaceResult.TotalTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceResultExists(RaceResult.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Race/Details", new { id =  raceResult.RaceId });
        }

        private bool RaceResultExists(Guid id)
        {
            return _context.RaceResult.Any(e => e.Id == id);
        }
    }
}
