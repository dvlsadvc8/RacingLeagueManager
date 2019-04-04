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

        //public IList<Data.Models.Penalty> Penalty { get;set; }
        public Data.Models.RaceResult RaceResult { get; set; }

        public async Task OnGetAsync(Guid raceResultId)
        {
            RaceResult = await _context.RaceResult
                .Include(p => p.Penalties)
                .Include(rr => rr.Race)
                    .ThenInclude(r => r.Track)
                .Include(rr => rr.Driver)
                .Where(rr => rr.Id == raceResultId).FirstOrDefaultAsync();

            
                //await _context.Penalty
                //.Include(p => p.RaceResult)
                //    .ThenInclude(rr => rr.Driver)
                //.Include(p => p.RaceResult)
                //    .ThenInclude(p => p.Race)
                //        .ThenInclude(r => r.Track)
                //.Where(p => p.RaceResultId == raceResultId).ToListAsync();
        }
    }
}
