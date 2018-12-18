using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RacingLeagueManager.Authorization
{
    public class RaceResultAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, RaceResult>
    {
        UserManager<Driver> _userManager;
        RacingLeagueManagerContext _context;

        public RaceResultAuthorizationHandler(RacingLeagueManagerContext context, UserManager<Driver>
            userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   RaceResult resource)
        {
            if (context.User == null || resource == null)
            {
                // Return Task.FromResult(0) if targeting a version of
                // .NET Framework older than 4.6:
                return Task.CompletedTask;
            }

            // If we're not asking for CRUD permission, return.

            //if (requirement.Name != Operations.Update.Name && requirement.Name != Operations.Delete.Name)
            //{
            //    return Task.CompletedTask;
            //}

            var race = _context.Race.Include(r => r.Series).FirstOrDefault(r => r.Id == resource.RaceId);
            var seriesEntry = _context.SeriesEntry.Include(s => s.SeriesEntryDrivers).Include(s => s.Team).FirstOrDefault(s => s.Id == resource.SeriesEntryId);

            var seriesId = race.Series.Id;
            var teamOwnerId = seriesEntry.Team.OwnerId;
            var userId = new Guid(_userManager.GetUserId(context.User));
            var ids = seriesEntry.SeriesEntryDrivers.Select(s => s.DriverId).ToList();

            if(race.Status == RaceStatus.Open)
            {
                if (ids.Any(s => s.Equals(userId))
                    || context.User.HasClaim("Role", "GlobalAdmin")
                    || context.User.HasClaim("SeriesAdmin", seriesId.ToString())
                    || teamOwnerId == userId)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
