using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesEntryDriver
{
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IList<Data.Models.SeriesEntryDriver> SeriesEntryDriver { get;set; }

        public async Task OnGetAsync()
        {
            SeriesEntryDriver = await _context.SeriesEntryDriver
                .Include(s => s.LeagueDriver)
                .Include(s => s.SeriesEntry).ToListAsync();
        }
    }
}
