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

namespace RacingLeagueManager.Pages.Leagues
{
    public class EditModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public EditModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public League League { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            League = await _context.League.FirstOrDefaultAsync(m => m.Id == id);

            if (League == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var league = await _context.League.FirstOrDefaultAsync(l => l.Id == League.Id);

            if(league == null)
            {
                return NotFound();
            }

            //_context.Attach(League).State = EntityState.Modified;

            league.Name = League.Name;
            league.Description = League.Description;

            await _context.SaveChangesAsync();

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!LeagueExists(League.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("/Leagues/Details", new { id = league.Id });
        }

        //private bool LeagueExists(Guid id)
        //{
        //    return _context.League.Any(e => e.Id == id);
        //}
    }
}
