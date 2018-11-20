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

namespace RacingLeagueManager.Pages.Penalty
{
    public class CreateModel : DI_BasePageModel
    {
        
        public CreateModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public async Task<IActionResult> OnGetAsync(Guid raceResultId)
        {
            RaceResult = await _context.RaceResult
                .Include(r => r.Race)
                    .ThenInclude(r => r.Track)
                .Include(r => r.Driver)
                .Include(r => r.SeriesEntry)
                    .ThenInclude(s => s.Series)
                .FirstOrDefaultAsync(r => r.Id == raceResultId);

            if(RaceResult == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, RaceResult.SeriesEntry.Series,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Penalty = new Data.Models.Penalty() { RaceResultId = raceResultId };

            return Page();
        }

        [BindProperty]
        public Data.Models.Penalty Penalty { get; set; }

        public Data.Models.RaceResult RaceResult { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var seriesEntry = _context.SeriesEntry.Include(s => s.Series).FirstOrDefault(s => s.Id == RaceResult.SeriesEntryId);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, seriesEntry.Series,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }


            _context.Penalty.Add(Penalty);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Race/Details", new { id = RaceResult.RaceId });
        }
    }
}