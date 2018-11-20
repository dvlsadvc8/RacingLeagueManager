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

namespace RacingLeagueManager.Pages.Team
{
    public class CreateModel : DI_BasePageModel
    {
        

        public CreateModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
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

            if(series.StartDate > DateTime.Now)
            {
                return Forbid();
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

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Team,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            _context.Team.Add(Team);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = Team.Id });
        }
    }
}