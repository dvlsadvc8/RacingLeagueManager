using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Penalty
{
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IList<Data.Models.Penalty> Penalty { get;set; }

        public async Task OnGetAsync(Guid raceResultId)
        {
            Penalty = await _context.Penalty
                .Include(p => p.RaceResult)
                    .ThenInclude(rr => rr.Driver)
                .Include(p => p.RaceResult)
                    .ThenInclude(p => p.Race)
                        .ThenInclude(r => r.Track)
                .Where(p => p.RaceResultId == raceResultId).ToListAsync();
        }
    }
}
