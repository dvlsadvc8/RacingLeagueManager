using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Rule
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid leagueId)
        {
            if(leagueId == null)
            {
                NotFound();
            }

            League league = await _context.League.Where(l => l.Id == leagueId).FirstOrDefaultAsync();

            Rule = new Data.Models.Rule() { LeagueId = leagueId, League = league };

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

            League league = await _context.League.Where(l => l.Id == Rule.LeagueId).Include(l => l.Rules).FirstOrDefaultAsync();
            Rule.Number = league.Rules.Count() + 1;

            _context.Rule.Add(Rule);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { leagueId = league.Id });
        }
    }
}