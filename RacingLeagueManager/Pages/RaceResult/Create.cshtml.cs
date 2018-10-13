using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.RaceResult
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid raceId, Guid seriesEntryId)
        {
            if(raceId == null || seriesEntryId == null)
            {
                return NotFound();
            }

            var teamDrivers = await _context.SeriesEntryDriver.Include(sed => sed.Driver).Where(sed => sed.SeriesEntryId == seriesEntryId).Select(sed => new { Id = sed.Driver.Id, UserName = string.Format("{0}-{1}", sed.Driver.UserName, sed.DriverType)}).ToListAsync();

            ViewData["DriverId"] = new SelectList(teamDrivers, "Id", "UserName");

            RaceResult = new Data.Models.RaceResult() { RaceId = raceId, SeriesEntryId = seriesEntryId };

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

            RaceResult.ResultType = ResultType.Finished;

            _context.RaceResult.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Race/Details", new { Id = RaceResult.RaceId });
        }

        public async Task<IActionResult> OnPostDnfAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            RaceResult.ResultType = ResultType.DNS;

            _context.RaceResult.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Race/Details", new { id = RaceResult.RaceId });
        }
    }
}