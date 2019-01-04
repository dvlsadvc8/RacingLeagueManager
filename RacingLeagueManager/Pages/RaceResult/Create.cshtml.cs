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
    public class CreateModel : DI_BasePageModel
    {

        public CreateModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public async Task<IActionResult> OnGetAsync(Guid raceId, Guid seriesEntryId)
        {
            if(raceId == null || seriesEntryId == null)
            {
                return NotFound();
            }

            //var user = await _userManager.GetUserAsync(User);
            //await _userManager.AddToRoleAsync(user, "GlobalAdmin");


            RaceResult = new Data.Models.RaceResult() { RaceId = raceId, SeriesEntryId = seriesEntryId };

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, RaceResult,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            var teamDrivers = await _context.SeriesEntryDriver.Include(sed => sed.Driver).Where(sed => sed.SeriesEntryId == seriesEntryId).Select(sed => new { Id = sed.Driver.Id, DisplayUserName = string.Format("{0}-{1}", sed.Driver.DisplayUserName, sed.DriverType)}).ToListAsync();

            ViewData["DriverId"] = new SelectList(teamDrivers, "Id", "DisplayUserName");

            

            return Page();
        }

        [BindProperty]
        public Data.Models.RaceResult RaceResult { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, RaceResult,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            RaceResult.ResultType = ResultType.Finished;

            _context.RaceResult.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Race/Details", new { Id = RaceResult.RaceId });
        }

        public async Task<IActionResult> OnPostRcAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, RaceResult,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            RaceResult.ResultType = ResultType.RC;

            _context.RaceResult.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Race/Details", new { id = RaceResult.RaceId });
        }

        public async Task<IActionResult> OnPostDnfAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, RaceResult,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            var race = await _context.Race.FirstOrDefaultAsync(r => r.Id == RaceResult.RaceId);

            RaceResult.TotalTime = RaceResult.BestLap * race.Laps;
            RaceResult.ResultType = ResultType.DNF;

            _context.RaceResult.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Race/Details", new { id = RaceResult.RaceId });
        }

        public async Task<IActionResult> OnPostDnsAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, RaceResult,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            RaceResult.ResultType = ResultType.DNS;

            _context.RaceResult.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Race/Details", new { id = RaceResult.RaceId });
        }
    }
}