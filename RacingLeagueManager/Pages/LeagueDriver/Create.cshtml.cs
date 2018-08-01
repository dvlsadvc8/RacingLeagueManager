using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.LeagueDriver
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["DriverId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["LeagueId"] = new SelectList(_context.League, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Data.Models.LeagueDriver LeagueDriver { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.LeagueDriver.Add(LeagueDriver);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}