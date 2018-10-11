using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Internal;
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

        //public Data.Models.Race Race { get; set; }
        public RaceDetailsViewModel RaceDetailsViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Race = await _context.Race
                //.Include(r => r.Results)
                .Include(r => r.Track)
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(s => s.SeriesEntryDrivers)
                            .ThenInclude(s => s.LeagueDriver)
                                .ThenInclude(ld => ld.Driver)
                
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(e => e.RaceResults)
                            .ThenInclude(r => r.Penalties)
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(s => s.Car)
                .Include(r => r.Series)
                    .ThenInclude(s => s.SeriesEntries)
                        .ThenInclude(s => s.Team)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Race == null)
            {
                return NotFound();
            }

            //Select((x, index) => new { index, x.Id, x.FirstName });

            List<RaceResultViewModel> ResultsList = new List<RaceResultViewModel>();
            foreach (var entry in Race.Series.SeriesEntries)
            {
                var raceResult = entry.RaceResults.FirstOrDefault(r => r.RaceId == Race.Id);

                RaceResultViewModel raceResultViewModel = new RaceResultViewModel()
                {
                    SeriesEntryId = entry.Id,
                    RaceNumber = entry.RaceNumber,
                    CarName = entry.Car.Name,
                    TeamName = entry.Team.Name,
                    //Points
                };

                if(raceResult != null)
                {
                    raceResultViewModel.RaceResultId = raceResult.Id;
                    raceResultViewModel.Username = raceResult.Driver.UserName;
                    raceResultViewModel.BestLap = raceResult.BestLap;
                    raceResultViewModel.TotalTime = raceResult.TotalTime;
                    raceResultViewModel.PenaltySeconds = raceResult.Penalties.Sum(p => p.Seconds);
                    raceResultViewModel.OfficialTime = raceResultViewModel.TotalTime?.Add(new TimeSpan(0,0,raceResultViewModel.PenaltySeconds));
                    raceResultViewModel.Place = raceResult.Place;
                    raceResultViewModel.ResultType = raceResult.ResultType;
                }

                ResultsList.Add(raceResultViewModel);
            }

            if (Race.Status != RaceStatus.Certified)
            {
                RaceDetailsViewModel = new RaceDetailsViewModel()
                {
                    RaceId = Race.Id,
                    TrackName = Race.Track.Name,
                    SeriesId = Race.SeriesId,
                    SeriesName = Race.Series.Name,
                    Laps = Race.Laps,
                    RaceDate = Race.RaceDate,
                    Status = Race.Status,
                    Results = ResultsList.Where(r => r.TotalTime != null).OrderBy(r => r.OfficialTime).Select((x, index) =>
                        new RaceResultViewModel()
                        {
                            Place = index + 1,
                            BestLap = x.BestLap,
                            CarName = x.CarName,
                            OfficialTime = x.OfficialTime,
                            PenaltySeconds = x.PenaltySeconds,
                            Points = x.Points,
                            RaceNumber = x.RaceNumber,
                            RaceResultId = x.RaceResultId,
                            SeriesEntryId = x.SeriesEntryId,
                            TeamName = x.TeamName,
                            TotalTime = x.TotalTime,
                            Username = x.Username,
                            ResultType = x.ResultType
                        }).ToList()
                };

                RaceDetailsViewModel.Results.AddRange(
                    ResultsList
                        .Where(r => r.TotalTime == null)
                        .OrderBy(r => r.Place)
                        .Select((x, index) =>
                            new RaceResultViewModel()
                            {
                                Place = RaceDetailsViewModel.Results.Count + 1,
                                BestLap = x.BestLap,
                                CarName = x.CarName,
                                OfficialTime = x.OfficialTime,
                                PenaltySeconds = x.PenaltySeconds,
                                Points = x.Points,
                                RaceNumber = x.RaceNumber,
                                RaceResultId = x.RaceResultId,
                                SeriesEntryId = x.SeriesEntryId,
                                TeamName = x.TeamName,
                                TotalTime = x.TotalTime,
                                Username = x.Username,
                                ResultType = x.ResultType
                            }
                        )
                );
            }
            else
            {
                RaceDetailsViewModel = new RaceDetailsViewModel()
                {
                    RaceId = Race.Id,
                    TrackName = Race.Track.Name,
                    SeriesId = Race.SeriesId,
                    SeriesName = Race.Series.Name,
                    Laps = Race.Laps,
                    RaceDate = Race.RaceDate,
                    Status = Race.Status,
                    Results = ResultsList.OrderBy(r => r.Place).Select(x =>
                        new RaceResultViewModel()
                        {
                            Place = x.Place,
                            BestLap = x.BestLap,
                            CarName = x.CarName,
                            OfficialTime = x.OfficialTime,
                            PenaltySeconds = x.PenaltySeconds,
                            Points = x.Points,
                            RaceNumber = x.RaceNumber,
                            RaceResultId = x.RaceResultId,
                            SeriesEntryId = x.SeriesEntryId,
                            TeamName = x.TeamName,
                            TotalTime = x.TotalTime,
                            Username = x.Username,
                            ResultType = x.ResultType
                        }).ToList()
                };
            }
            

            return Page();
        }

        public async Task<IActionResult> OnPostStatusAsync(Guid raceId)
        {
            if(raceId == null)
            {
                return NotFound();
            }

            var race = await _context.Race.FirstOrDefaultAsync(r => r.Id == raceId);

            if(race == null)
            {
                return NotFound();
            }

            race.Status += 1;

            await _context.SaveChangesAsync();

            return RedirectToPage("Details", new { id = raceId });
        }
    }

    public class RaceDetailsViewModel
    {
        public Guid RaceId { get; set; }
        public string TrackName { get; set; }
        public Guid SeriesId { get; set; }
        public string SeriesName { get; set; }
        public int Laps { get; set; }
        public DateTime RaceDate { get; set; }
        public RaceStatus? Status { get; set; }

        public List<RaceResultViewModel> Results { get; set; }
        public string GetStatusButtonHtml()
        {
            string statusButtonHtml = string.Empty;
            switch(this.Status)
            {
                case RaceStatus.Pending:
                    statusButtonHtml = "<input type='submit' value='Open Race' class='btn-link' />";
                    break;
                case RaceStatus.Open:
                    statusButtonHtml = "<input type='submit' value='Close Race' class='btn-link' />";
                    break;
                case RaceStatus.Closed:
                    statusButtonHtml = "<input type='submit' value='Certify Race' class='btn-link' />";
                    break;
                default:
                    break;
            }

            return statusButtonHtml;
        }

        public string GetStatusCssClass()
        {
            string statusCssClass = string.Empty;
            switch (this.Status)
            {
                case RaceStatus.Pending:
                    statusCssClass = "label label-default";
                    break;
                case RaceStatus.Open:
                    statusCssClass = "label label-success";
                    break;
                case RaceStatus.Closed:
                    statusCssClass = "label label-danger";
                    break;
                default:
                    statusCssClass = "label label-info";
                    break;
            }

            return statusCssClass;
        }
    }

    public class RaceResultViewModel
    {
        public Guid RaceResultId { get; set; }
        public Guid SeriesEntryId { get; set; }

        public int Place { get; set; }
        public string Username { get; set; }
        public string RaceNumber { get; set; }
        public string CarName { get; set; }
        public string TeamName { get; set; }
        public int Points { get; set; }
        public TimeSpan? BestLap { get; set; }
        public TimeSpan? TotalTime { get; set; }
        public int PenaltySeconds { get; set; }
        public TimeSpan? OfficialTime { get; set; }
        public ResultType? ResultType { get; set; }
    }
}
