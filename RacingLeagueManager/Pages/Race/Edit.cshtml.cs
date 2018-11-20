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

namespace RacingLeagueManager.Pages.Race
{
    public class EditModel : DI_BasePageModel
    {
       
        public EditModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Data.Models.Race Race { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Race = await _context.Race.Include(r => r.Series)
                .Include(r => r.Track).FirstOrDefaultAsync(m => m.Id == id);

            if (Race == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Race.Series,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            ViewData["TrackId"] = new SelectList(_context.Track, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var race = await _context.Race.Include(r => r.Series)
                .Include(r => r.Track).FirstOrDefaultAsync(m => m.Id == Race.Id);

            if (race == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, race.Series,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            race.Laps = Race.Laps;
            race.RaceDate = Race.RaceDate;
            race.TrackId = Race.TrackId;

            await _context.SaveChangesAsync();

            //_context.Attach(Race).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!RaceExists(Race.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Index");
        }

        private bool RaceExists(Guid id)
        {
            return _context.Race.Any(e => e.Id == id);
        }
    }
}
