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
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public async Task<IActionResult> OnGetAsync(Guid leagueId)
        {
            if(leagueId == null)
            {
                NotFound();
            }

            League league = await _context.League.Where(l => l.Id == leagueId).FirstOrDefaultAsync();

            Rule = new Data.Models.Rule() { LeagueId = leagueId, League = league };

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Rule,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }


            return Page();
        }

        [BindProperty]
        public Data.Models.Rule Rule { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int ruleCount = await _context.Rule.Where(r => r.LeagueId == Rule.LeagueId).CountAsync();
            Rule.Number = ruleCount + 1; //league.Rules.Count() + 1;

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, Rule,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            _context.Rule.Add(Rule);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { leagueId = Rule.LeagueId });
        }
    }
}