using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesDriver
{
    public class IndexModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public IndexModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IList<Data.Models.SeriesDriver> SeriesDriver { get;set; }

        public async Task OnGetAsync(Guid? seriesId)
        {
            if(seriesId == null)
            {
                BadRequest();
            }

            SeriesDriver = await _context.SeriesDriver
                .Include(s => s.LeagueDriver)
                    .ThenInclude(ld => ld.Driver)
                .Include(s => s.Series)
                .Where(s => s.SeriesId == seriesId.Value)
                .OrderBy(s => s.LeagueDriver.PreQualifiedTime).ToListAsync();
        }
    }
}
