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

namespace RacingLeagueManager.Pages.Rule
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
        public Data.Models.Rule Rule { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rule = await _context.Rule
                .Include(r => r.League).FirstOrDefaultAsync(m => m.Id == id);

            if (Rule == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Rule,
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

            var rule = await _context.Rule
                .Include(r => r.League).FirstOrDefaultAsync(m => m.Id == Rule.Id);

            if(rule == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, rule,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            rule.Number = Rule.Number;
            rule.Description = Rule.Description;
            //_context.Attach(Rule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RuleExists(Rule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RuleExists(Guid id)
        {
            return _context.Rule.Any(e => e.Id == id);
        }
    }
}
