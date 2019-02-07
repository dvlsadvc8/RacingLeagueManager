using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;
using RacingLeagueManager.Pages.Shared;

namespace RacingLeagueManager.Pages
{
    [AllowAnonymous]

    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager) : base(context, authorizationService, userManager)
        {
        }

        public IList<Data.Models.Race> Races { get; set; }

        public string GetStatusCssClass(RaceStatus raceStatus)
        {
            string statusCssClass = string.Empty;
            switch (raceStatus)
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

        public async Task OnGetAsync()
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                Races = await _context.Race
                    .Include(r => r.Series)
                        .ThenInclude(s => s.SeriesDrivers)
                    .Include(r => r.Track)
                    .Where(r => r.Series.SeriesDrivers.Any(sd => sd.DriverId == user.Id)
                        && r.RaceDate > DateTime.Now.AddDays(-10)
                        && r.RaceDate < DateTime.Now.AddDays(20))
                    .OrderBy(r => r.RaceDate)
                    .ToListAsync();

            }
        }
    }
}
