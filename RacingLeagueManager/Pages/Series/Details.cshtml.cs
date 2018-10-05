using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Series
{
    public class DetailsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DetailsModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public Data.Models.Series Series { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Series = await _context.Series
                .Include(s => s.Owner)
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

            return Page();
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
