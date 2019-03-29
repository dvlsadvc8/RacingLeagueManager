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

namespace RacingLeagueManager.Pages.Penalty
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
        public Data.Models.Penalty Penalty { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Penalty = await _context.Penalty
                .Include(p => p.RaceResult)
                    .ThenInclude(r => r.SeriesEntry)
                        .ThenInclude(s => s.Series)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Penalty == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Penalty.RaceResult.SeriesEntry.Series,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(Penalty).State = EntityState.Modified;

            var penalty = await _context.Penalty
                .Include(p => p.RaceResult)
                    .ThenInclude(r => r.SeriesEntry)
                        .ThenInclude(s => s.Series)
                .FirstOrDefaultAsync(m => m.Id == Penalty.Id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, penalty.RaceResult.SeriesEntry.Series,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            penalty.Seconds = Penalty.Seconds;
            penalty.Description = Penalty.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PenaltyExists(penalty.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { raceResultId = penalty.RaceResultId });
        }

        private bool PenaltyExists(Guid id)
        {
            return _context.Penalty.Any(e => e.Id == id);
        }
    }
}
