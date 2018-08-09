﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Pages.SeriesEntry
{
    public class CreateModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public CreateModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid seriesId)
        {
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Name");
            //ViewData["SeriesId"] = new SelectList(_context.Series, "Id", "Id");

            SeriesEntry = new Data.Models.SeriesEntry() { SeriesId = seriesId };

            return Page();
        }

        [BindProperty]
        public Data.Models.SeriesEntry SeriesEntry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SeriesEntry.Add(SeriesEntry);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}