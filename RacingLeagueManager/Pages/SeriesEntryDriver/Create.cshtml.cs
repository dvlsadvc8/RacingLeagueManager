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

namespace RacingLeagueManager.Pages.SeriesEntryDriver
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(Guid seriesEntryId)
        {
            var seriesEntry = await _context.SeriesEntry.Include(s => s.Series).Include(s => s.Team).FirstOrDefaultAsync(s => s.Id == seriesEntryId);
            if(seriesEntry == null)
            {
                return NotFound();
            }

            var leagueId = seriesEntry.Series.LeagueId;



            //var takenDriverList = await _context.LeagueDriver
            //    .Include(l => l.SeriesEntryDrivers)
            //    .Where(l => l.SeriesEntryDrivers.Any(s => s.SeriesEntry.SeriesId == seriesEntry.SeriesId)).ToListAsync();


            var availableDriverList = await _context.SeriesDriver
                .Where(s => s.SeriesId == seriesEntry.SeriesId && s.Status == "Available")
                .Include(s => s.LeagueDriver)
                    .ThenInclude(ld => ld.Driver)
                .OrderBy(s => s.LeagueDriver.PreQualifiedTime)
                .Select(s => new { DriverId = s.DriverId, DisplayValue = s.LeagueDriver.PreQualifiedTime.ToString(@"mm\:ss\.fff") + " " + s.LeagueDriver.Driver.UserName }).ToListAsync();

            //var availableDriverList = await _context.LeagueDriver

            //    .Where(l => l.LeagueId == leagueId)
            //    .Include(l => l.Driver)
            //    .Except(takenDriverList)
            //    .OrderBy(s => s.PreQualifiedTime)
            //    .Select(s => new { DriverId = s.DriverId, DisplayValue = s.PreQualifiedTime + " " + s.Driver.UserName}).ToListAsync();

            ViewData["DriverId"] = new SelectList(availableDriverList, "DriverId", "DisplayValue");

            //ViewData["DriverId"] = new SelectList(await _context.LeagueDriver.Include(l => l.Driver).Where(l => l.LeagueId == seriesEntry.Series.LeagueId).ToListAsync(), "DriverId", "Driver.UserName");
            //ViewData["TeamId"] = seriesEntry.Team.Id;

            SeriesEntryDriver = new Data.Models.SeriesEntryDriver() { SeriesEntryId = seriesEntry.Id, LeagueId = seriesEntry.Series.LeagueId, SeriesEntry = seriesEntry};
            SeriesId = seriesEntry.Series.Id;

            return Page();
        }

        [BindProperty]
        public Data.Models.SeriesEntryDriver SeriesEntryDriver { get; set; }
        [BindProperty]
        public Guid SeriesId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var teamId = SeriesEntryDriver.SeriesEntry.Team.Id;
            var seriesId = SeriesEntryDriver.SeriesEntry.SeriesId;
            SeriesEntryDriver.SeriesEntry = null;

            _context.SeriesEntryDriver.Add(SeriesEntryDriver);
            var driver = await _context.SeriesDriver.FirstOrDefaultAsync(s => s.SeriesId == SeriesId && s.DriverId == SeriesEntryDriver.DriverId);

            if(SeriesEntryDriver.DriverType == DriverType.Primary)
            {
                driver.Status = "Unavailable - Primary";
            }
            else if(SeriesEntryDriver.DriverType == DriverType.Reserve)
            {
                driver.Status = "Available - Reserve";
            }
            else
            {
                driver.Status = "Unavailable - Retired";
            }
            

            await _context.SaveChangesAsync();

            return RedirectToPage("../Team/Details", new { id = teamId });
        }
    }
}