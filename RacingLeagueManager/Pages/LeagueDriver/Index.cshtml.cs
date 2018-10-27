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

        //public IList<Data.Models.LeagueDriver> LeagueDriver { get;set; }
        public LeagueDriverIndexViewModel LeagueDrivers { get; set; }

        public async Task OnGetAsync(Guid leagueId)
        {
            if(leagueId == null)
            {
                NotFound();
            }

            var league = await _context.League
                .Include(l => l.LeagueDrivers)
                    .ThenInclude(ld => ld.Driver)
                .FirstOrDefaultAsync(l => l.Id == leagueId);

            if(league == null)
            {
                NotFound();
            }

            //var drivers = await _context.LeagueDriver
            //    .Include(l => l.Driver)
            //    .Include(l => l.League)
            //    .Where(l => l.LeagueId == leagueId)
            //    .ToListAsync();

            LeagueDrivers = new LeagueDriverIndexViewModel();

            LeagueDrivers.LeagueId = league.Id;
            LeagueDrivers.LeagueName = league.Name;
            LeagueDrivers.ActiveDrivers = league.LeagueDrivers.Where(d => d.Status == "Active" || d.Status == null).ToList();
            LeagueDrivers.PendingDrivers = league.LeagueDrivers.Where(d => d.Status == "Pending").ToList();
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid leagueId, Guid driverId)
        {
            if(leagueId == null || driverId == null)
            {
                NotFound();
            }

            var leagueDriver = await _context.LeagueDriver.FirstOrDefaultAsync(l => l.LeagueId == leagueId && l.DriverId == driverId);

            if(leagueDriver == null)
            {
                NotFound();
            }

            leagueDriver.Status = "Active";

            await _context.SaveChangesAsync();

            return RedirectToPage("/LeagueDriver/Index", new { leagueId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid leagueId, Guid driverId)
        {
            if (leagueId == null || driverId == null)
            {
                NotFound();
            }

            var leagueDriver = await _context.LeagueDriver.FirstOrDefaultAsync(l => l.LeagueId == leagueId && l.DriverId == driverId);

            if (leagueDriver == null)
            {
                NotFound();
            }

            _context.LeagueDriver.Remove(leagueDriver);

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }

    public class LeagueDriverIndexViewModel
    {
        public Guid LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<Data.Models.LeagueDriver> ActiveDrivers { get; set; }
        public IList<Data.Models.LeagueDriver> PendingDrivers { get; set; }
    }
}
