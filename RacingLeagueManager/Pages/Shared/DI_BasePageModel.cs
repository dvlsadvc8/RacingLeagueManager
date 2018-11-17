using RacingLeagueManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RacingLeagueManager.Data;

namespace RacingLeagueManager.Pages.Shared
{
    public class DI_BasePageModel : PageModel
    {
        protected RacingLeagueManagerContext _context { get; }
        protected IAuthorizationService _authorizationService { get; }
        protected UserManager<Driver> _userManager { get; }

        public DI_BasePageModel(
            RacingLeagueManagerContext context,
            IAuthorizationService authorizationService,
            UserManager<Driver> userManager) : base()
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }
    }
}
