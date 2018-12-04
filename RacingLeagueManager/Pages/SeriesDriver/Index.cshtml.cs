using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        //public IList<Data.Models.SeriesDriver> SeriesDriver { get;set; }
        public IList<SeriesDriverIndexViewModel> SeriesDrivers { get; set; }
        public Guid SeriesId { get; set; }

        public async Task OnGetAsync(Guid? seriesId)
        {
            if(seriesId == null)
            {
                BadRequest();
            }

            SeriesId = seriesId.Value;

            var data = await _context.SeriesDriver
                .Include(s => s.LeagueDriver)
                    .ThenInclude(ld => ld.Driver)
                .Include(s => s.Series)
                .Where(s => s.SeriesId == seriesId.Value)
                .OrderBy(s => s.LeagueDriver.PreQualifiedTime)
                .ToListAsync();

            SeriesDrivers = data
                .Select((x, i) => new SeriesDriverIndexViewModel()
                {
                    Rank = i + 1,
                    DriverId = x.DriverId,
                    LeagueId = x.LeagueId,
                    DisplayUserName = x.LeagueDriver.Driver.DisplayUserName,
                    PreQualifiedTime = x.LeagueDriver.PreQualifiedTime,
                    Status = x.Status,
                    SeriesName = x.Series.Name
                })
                .ToList();
        }

        
    }

    public class SeriesDriverIndexViewModel
    {
        public int Rank { get; set; }
        public Guid LeagueId { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public string DisplayUserName { get; set; }
        [DisplayFormat(DataFormatString = "{0:mm\\:ss\\.fff}", ApplyFormatInEditMode = true)]
        public TimeSpan PreQualifiedTime { get; set; }
        public string Status { get; set; }
        public string SeriesName { get; set; }
    }
}
