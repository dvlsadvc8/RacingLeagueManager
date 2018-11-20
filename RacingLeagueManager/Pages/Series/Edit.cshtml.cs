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

namespace RacingLeagueManager.Pages.Series
{
    public class EditModel : DI_BasePageModel
    {

        public EditModel(
            RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Data.Models.Series Series { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Series = await _context.Series
                .Include(s => s.League).FirstOrDefaultAsync(m => m.Id == id);

            if (Series == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Series,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            ViewData["LeagueId"] = new SelectList(_context.League, "Id", "Id");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var series = await _context.Series.FirstOrDefaultAsync(l => l.Id == Series.Id);

            if (series == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Series,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            //_context.Attach(League).State = EntityState.Modified;

            series.Name = Series.Name;
            series.Description = Series.Description;
            series.StartDate = Series.StartDate;

            await _context.SaveChangesAsync();

            //_context.Attach(Series).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!SeriesExists(Series.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Details", new { id = Series.Id });
        }

        private bool SeriesExists(Guid id)
        {
            return _context.Series.Any(e => e.Id == id);
        }
    }
}
