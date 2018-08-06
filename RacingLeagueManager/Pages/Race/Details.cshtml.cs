﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.Race
{
    public class DetailsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public DetailsModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public Data.Models.Race Race { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceQuery = _context.Race
                .Include(r => r.Results)
                .Include(r => r.Track)
                .Include(r => r.Series)
                    .ThenInclude(s => s.Entries.Select(e => !_context.RaceResult.Any(r => r.SeriesId == e.SeriesId && r.LeagueId == e.LeagueId && r.DriverId == e.DriverId)));
                    
            Race = await raceQuery.FirstOrDefaultAsync(m => m.Id == id);

            if (Race == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
