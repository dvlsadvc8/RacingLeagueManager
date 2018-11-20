﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using RacingLeagueManager.Data;
using RacingLeagueManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RacingLeagueManager.Authorization
{
    public class SeriesDriverAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, SeriesDriver>
    {
        UserManager<Driver> _userManager;
        RacingLeagueManagerContext _context;

        public SeriesDriverAuthorizationHandler(UserManager<Driver>
            userManager, RacingLeagueManagerContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   SeriesDriver resource)
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

            if(requirement.Name == Operations.Create.Name)
            {
                if(_context.LeagueDriver.Any(l => l.DriverId == resource.DriverId && l.LeagueId == resource.LeagueId && l.Status == "Active"))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                if (context.User.HasClaim("Role", "GlobalAdmin")
                || context.User.HasClaim("LeagueAdmin", resource.LeagueId.ToString())
                || context.User.HasClaim("SeriesAdmin", resource.SeriesId.ToString()))
                {
                    context.Succeed(requirement);
                }
            }

            

            return Task.CompletedTask;
        }
    }
}
