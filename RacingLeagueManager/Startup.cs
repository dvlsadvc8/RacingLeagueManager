using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RacingLeagueManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using RacingLeagueManager.Authorization;

namespace RacingLeagueManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));

            if (Environment.EnvironmentName == "Production")
                services.AddDbContext<RacingLeagueManagerContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            else

                services.AddDbContext<RacingLeagueManagerContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<Driver>()
                //.AddRoles<Role>()
                //.AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<RacingLeagueManagerContext>();

            //services.AddIdentity<Driver, Role>()
                //.AddRoles<Role>()
                //.AddRoleManager<RoleManager<Role>>()
                //.AddEntityFrameworkStores<RacingLeagueManagerContext>();

            // Automatically perform database migration
            services.BuildServiceProvider().GetService<RacingLeagueManagerContext>().Database.Migrate();


            var skipHTTPS = Configuration.GetValue<bool>("LocalTest:skipHTTPS");
            services.Configure<MvcOptions>(options =>
            {
                // Set LocalTest:skipHTTPS to true to skip SSL requirement in debug mode.
                if (Environment.IsDevelopment() && !skipHTTPS)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }
            });


            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Authorization handlers.
            services.AddScoped<IAuthorizationHandler, LeagueAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, SeriesAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, LeagueDriverAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RuleAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, SeriesDriverAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, TeamAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RaceResultAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }

        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    //initializing custom roles 
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<Driver>>();
        //    string[] roleNames = { "Admin", "Manager", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            //create the roles and seed them to the database: Question 1
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }

        //    //Here you could create a super user who will maintain the web app
        //    var poweruser = new Driver
        //    {

        //        UserName = Configuration["AppSettings:UserName"],
        //        Email = Configuration["AppSettings:UserEmail"],
        //    };
        //    //Ensure you have these values in your appsettings.json file
        //    string userPWD = Configuration["AppSettings:UserPassword"];
        //    var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

        //    if (_user == null)
        //    {
        //        var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
        //        if (createPowerUser.Succeeded)
        //        {
        //            //here we tie the new user to the role
        //            await UserManager.AddToRoleAsync(poweruser, "Admin");

        //        }
        //    }
        //}
    }
}
