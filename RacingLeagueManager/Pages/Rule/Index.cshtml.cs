using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Rule
{
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        //public IList<Data.Models.Rule> Rule { get;set; }

        public Data.Models.League League { get; set; }

        public async Task OnGetAsync(Guid leagueId)
        {
            if(leagueId == null)
            {
                NotFound();
            }

            League = await _context.League
                .Where(r => r.Id == leagueId)
                .Include(r => r.Rules).FirstOrDefaultAsync();


        }
    }
}
