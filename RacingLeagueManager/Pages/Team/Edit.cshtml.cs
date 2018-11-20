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

namespace RacingLeagueManager.Pages.Team
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
        public Data.Models.Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Team
                .Include(t => t.Owner)
                .Include(t => t.Series).FirstOrDefaultAsync(m => m.Id == id);

            if (Team == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Team,
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

            var team = await _context.Team.FirstOrDefaultAsync(t => t.Id == Team.Id);

            team.Name = Team.Name;

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, team,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }


            await _context.SaveChangesAsync();

            //_context.Attach(Team).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!TeamExists(Team.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Details", new { id = team.Id });
        }

        //private bool TeamExists(Guid id)
        //{
        //    return _context.Team.Any(e => e.Id == id);
        //}
    }
}
