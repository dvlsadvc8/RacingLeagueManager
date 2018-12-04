using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Authorization;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;
using RacingLeagueManager.Pages.Shared;

namespace RacingLeagueManager.Pages.LeagueDriver
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        //public IList<Data.Models.LeagueDriver> LeagueDriver { get;set; }
        public LeagueDriverIndexViewModel LeagueDrivers { get; set; }
        public Guid LeagueId { get; set; }

        public async Task OnGetAsync(Guid leagueId)
        {
            if(leagueId == null)
            {
                NotFound();
            }

            LeagueId = leagueId;

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
            LeagueDrivers.ActiveDrivers = league.LeagueDrivers.Where(d => d.Status == "Active" || d.Status == null)
                .OrderBy(d => d.PreQualifiedTime)
                .Select((x,i) => new LeagueDriverViewModel()
                {
                    Rank = i + 1,
                    DisplayUserName = x.Driver.DisplayUserName,

                    DriverId = x.DriverId,
                    PreQualifiedTime = x.PreQualifiedTime
                }).ToList();

            LeagueDrivers.PendingDrivers = league.LeagueDrivers.Where(d => d.Status == "Pending").ToList();
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid leagueId, Guid driverId)
        {
            if(leagueId == null || driverId == null)
            {
                NotFound();
            }

            var leagueDriver = await _context.LeagueDriver.Include(l => l.League).FirstOrDefaultAsync(l => l.LeagueId == leagueId && l.DriverId == driverId);

            if(leagueDriver == null)
            {
                NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, leagueDriver.League,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
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

            var leagueDriver = await _context.LeagueDriver.Include(l => l.League).FirstOrDefaultAsync(l => l.LeagueId == leagueId && l.DriverId == driverId);

            if (leagueDriver == null)
            {
                NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                User, leagueDriver.League,
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
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
        public IList<LeagueDriverViewModel> ActiveDrivers { get; set; }
        public IList<Data.Models.LeagueDriver> PendingDrivers { get; set; }
    }

    public class LeagueDriverViewModel
    {
        [Display(Name = "Rank")]
        public int Rank { get; set; }
        public Guid DriverId { get; set; }
        [Display(Name ="Member Name")]
        public string DriverName { get; set; }
        [Display(Name = "Member Name")]
        public string DisplayUserName { get; set; }
        [DisplayFormat(DataFormatString = "{0:mm\\:ss\\.fff}", ApplyFormatInEditMode = true)]
        [Display(Name ="Prequalifier Time")]
        public TimeSpan PreQualifiedTime { get; set; }
        
    }
}
