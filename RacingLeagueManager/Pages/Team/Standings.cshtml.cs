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

namespace RacingLeagueManager.Pages.Team
{
    public class StandingsModel : PageModel
    {
        private readonly RacingLeagueManager.Data.RacingLeagueManagerContext _context;

        public StandingsModel(RacingLeagueManager.Data.RacingLeagueManagerContext context)
        {
            _context = context;
        }

        //public IList<Data.Models.Team> Team { get; set; }
        public List<TeamStandingsViewModel> TeamStandingsViewModel { get; set; }
        public string SeriesName { get; set; }
        public Guid SeriesId { get; set; }

        public async Task OnGetAsync(Guid seriesId)
        {
            var teams = await _context.Team
                .Include(t => t.SeriesEntries)
                    .ThenInclude(s => s.RaceResults)
                .Include(t => t.SeriesEntries)
                    .ThenInclude(s => s.Car)
                .Include(t => t.Owner)
                .Include(t => t.Series)
                .Where(t => t.SeriesId == seriesId)
                .ToListAsync();

            var series = teams.FirstOrDefault().Series;
            SeriesName = series.Name;
            SeriesId = series.Id;
            
            TeamStandingsViewModel = BuildModel(teams);
        }

        

        private List<TeamStandingsViewModel> BuildModel(List<Data.Models.Team> teams)
        {
            List<TeamStandingsViewModel> modelList = new List<TeamStandingsViewModel>();






            modelList = teams
                .Select(x =>
                new TeamStandingsViewModel()
                {
                    CarName = x.SeriesEntries.FirstOrDefault().Car.Name,
                    TeamName = x.Name,
                    Points = x.SeriesEntries.Sum(s => s.RaceResults.Sum(r => r.Points))
                }
            ).ToList();

            modelList = modelList.OrderByDescending(x => x.Points)
                            .Select((x, i) =>
                            new Team.TeamStandingsViewModel()
                            {
                                Place = i + 1,
                                CarName = x.CarName,
                                TeamName = x.TeamName,
                                Points = x.Points
                            }).ToList();

            //var standings = modelList.OrderByDescending(x => x.Points).Select((x, i) =>
            //    new StandingsViewModel()
            //    {
            //        Place = i + 1,
            //        RaceNumber = x.RaceNumber,
            //        Car = x.Car,
            //        Drivers = x.Drivers,
            //        Team = x.Team,
            //        Points = x.Points
            //    }
            //);

            return modelList;
        }
    }

    public class TeamStandingsViewModel
    {
        public int Place { get; set; }
        [Display(Name ="Team Name")]
        public string TeamName { get; set; }
        [Display(Name = "Car")]
        public string CarName { get; set; }
        public int Points { get; set; }
        
    }
}
