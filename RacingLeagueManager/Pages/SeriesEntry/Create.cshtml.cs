using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Authorization;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;
using RacingLeagueManager.Pages.Shared;

namespace RacingLeagueManager.Pages.SeriesEntry
{
    public class CreateModel : DI_BasePageModel
    {
        

        public CreateModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public async Task<IActionResult> OnGetAsync(Guid seriesId, Guid teamId)
        {
            if(seriesId == null || teamId == null)
            {
                return NotFound();
            }

            var series = _context.Series.FirstOrDefault(s => s.Id == seriesId);
            var team = _context.Team.FirstOrDefault(t => t.Id == teamId);

            if (series == null || team == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, team,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            

            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Name");

            SeriesEntry = new Data.Models.SeriesEntry() { SeriesId = seriesId, Series = series, TeamId = teamId, Team = team };

            return Page();
        }

        [BindProperty]
        public Data.Models.SeriesEntry SeriesEntry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if(_context.SeriesEntry.Any(s => s.RaceNumber == SeriesEntry.RaceNumber && s.SeriesId == SeriesEntry.SeriesId))
            {
                ModelState.AddModelError("RaceNumber", "This number is already taken.");
            }

            var series = _context.Series.FirstOrDefault(s => s.Id == SeriesEntry.SeriesId);
            var team = _context.Team.FirstOrDefault(t => t.Id == SeriesEntry.TeamId);

            if (!ModelState.IsValid)
            {

                SeriesEntry.Team = team;
                SeriesEntry.Series = series;

                ViewData["CarId"] = new SelectList(_context.Car, "Id", "Name");
                return Page();
            }

            

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, team,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }


            _context.SeriesEntry.Add(SeriesEntry);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Team/Details", new { id = SeriesEntry.TeamId });
        }
    }
}