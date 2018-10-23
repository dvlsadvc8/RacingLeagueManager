using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesDriver
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;
        private readonly UserManager<Driver> _userManager;

        public CreateModel(UserManager<Driver> userManager, RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid seriesId)
        {
            var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);

            SeriesDriver = new Data.Models.SeriesDriver()
            {
                SeriesId = seriesId,
                LeagueId = series.LeagueId
            };

            return Page();
        }

        [BindProperty]
        public Data.Models.SeriesDriver SeriesDriver { get; set; }

        public async Task<IActionResult> OnPostAsync(Guid? seriesId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(seriesId == null)
            {
                return BadRequest();
            }

            Data.Models.Series series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);
            Driver driver = await _userManager.GetUserAsync(User);

            SeriesDriver.SeriesId = seriesId.Value;
            SeriesDriver.LeagueId = series.LeagueId;
            SeriesDriver.DriverId = driver.Id;
            SeriesDriver.Status = "Available";


            _context.SeriesDriver.Add(SeriesDriver);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { seriesId = series.Id });
        }
    }
}