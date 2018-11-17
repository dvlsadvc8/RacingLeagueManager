﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using RacingLeagueManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RacingLeagueManager.Authorization
{
    public class RuleAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, Rule>
    {
        UserManager<Driver> _userManager;

        public RuleAuthorizationHandler(UserManager<Driver>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Rule resource)
        {
            if (context.User == null || resource == null || resource.League == null)
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



            if (resource.League.OwnerId == new Guid(_userManager.GetUserId(context.User)) 
                ||context.User.IsInRole("GlobalAdmin") 
                || context.User.HasClaim("LeagueAdmin", resource.Id.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
