using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesEntry
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

        public IActionResult OnGet(Guid seriesId)
        {
            Data.Models.Series series = _context.Series.FirstOrDefault(s => s.Id == seriesId);
            if(series == null)
            {
                return NotFound();
            }

            SeriesEntry = new Data.Models.SeriesEntry() { SeriesId = series.Id };
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Name");
            //ViewData["DriverId"] = new SelectList(_context.Users, "Id", "Id");
            //ViewData["SeriesId"] = new SelectList(_context.Series, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Data.Models.SeriesEntry SeriesEntry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Driver driver = await _userManager.GetUserAsync(User);
            SeriesEntry.DriverId = driver.Id;

            _context.SeriesEntry.Add(SeriesEntry);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Series/Details");
        }
    }
}