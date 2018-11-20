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

namespace RacingLeagueManager.Pages.Series
{
    public class DetailsModel : DI_BasePageModel
    {

        public DetailsModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public Data.Models.Series Series { get; set; }
        public bool IsJoinable { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Series = await _context.Series
                .Include(s => s.Owner)
                .Include(s => s.League)
                .Include(s => s.SeriesDrivers)
                .Include(s => s.SeriesEntries)
                    .ThenInclude(s => s.Car)
                .Include(s => s.SeriesEntries)
                    .ThenInclude(s => s.Team)
                .Include(s => s.SeriesEntries)
                    .ThenInclude(s => s.SeriesEntryDrivers)
                        .ThenInclude(s => s.LeagueDriver)
                            .ThenInclude(ld => ld.Driver)
                .Include(s => s.Races)
                    .ThenInclude(s => s.Track)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Series == null)
            {
                return NotFound();
            }

            Driver driver = await _userManager.GetUserAsync(User);
            IsJoinable = !Series.SeriesDrivers.Any(sd => sd.DriverId == driver.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostJoinAsync(Guid? seriesId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (seriesId == null)
            {
                return BadRequest();
            }

            Data.Models.Series series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);
            Driver driver = await _userManager.GetUserAsync(User);

            Data.Models.SeriesDriver seriesDriver = new Data.Models.SeriesDriver()
            {
                SeriesId = seriesId.Value,
                LeagueId = series.LeagueId,
                DriverId = driver.Id,
                Status = "Available"
            };

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, seriesDriver,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }


            _context.SeriesDriver.Add(seriesDriver);
            await _context.SaveChangesAsync();

            return RedirectToPage("/SeriesDriver/Index", new { seriesId = series.Id });
        }

        public string GetStatusCssClass(RaceStatus? status)
        {
            string statusCssClass = string.Empty;
            switch (status)
            {
                case RaceStatus.Pending:
                    statusCssClass = "label label-default";
                    break;
                case RaceStatus.Open:
                    statusCssClass = "label label-success";
                    break;
                case RaceStatus.Closed:
                    statusCssClass = "label label-danger";
                    break;
                default:
                    statusCssClass = "label label-info";
                    break;
            }

            return statusCssClass;
        }
    }
}
