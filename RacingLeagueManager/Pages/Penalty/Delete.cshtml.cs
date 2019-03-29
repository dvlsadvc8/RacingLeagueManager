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

namespace RacingLeagueManager.Pages.Penalty
{
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(RacingLeagueManagerContext context,
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
                                                Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
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
                                                Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            _context.Penalty.Remove(Penalty);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { raceResultId = Penalty.RaceResultId });
        }
    }
}
