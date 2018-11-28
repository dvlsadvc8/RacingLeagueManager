using Microsoft.AspNetCore.Authorization;
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
    public class RuleAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, Rule>
    {
        private UserManager<Driver> _userManager;
        private RacingLeagueManagerContext _context;

        public RuleAuthorizationHandler(RacingLeagueManagerContext context, UserManager<Driver>
            userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Rule resource)
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

            var ownerId = _context.League.SingleOrDefault(l => l.Id == resource.LeagueId).OwnerId;

            if (ownerId == new Guid(_userManager.GetUserId(context.User)) 
                || context.User.HasClaim("Role", "GlobalAdmin")
                || context.User.HasClaim("LeagueAdmin", resource.LeagueId.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
