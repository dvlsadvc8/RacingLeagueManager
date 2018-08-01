using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.LeagueDriver
{
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IList<Data.Models.LeagueDriver> LeagueDriver { get;set; }

        public async Task OnGetAsync()
        {
            LeagueDriver = await _context.LeagueDriver
                .Include(l => l.Driver)
                .Include(l => l.League).ToListAsync();
        }
    }
}
