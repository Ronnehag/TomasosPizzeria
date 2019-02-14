using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Services;

namespace TomasosPizzeria
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProduktService, ProduktService>();

            services.AddDbContext<TomasosContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionString"]));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings for Core Identity users, dumbed down for testing purpose.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/user/login";
                options.SlidingExpiration = true;
            });

            services.AddDistributedMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseMvc(route =>
            {
                route.MapRoute(
                    name: "default",
                    template: "{Controller=Home}/{Action=Index}"
                );

            });

            // Creates the IdentityRoles, will check if they don't already exist.
            // Also creates the Admin account. Remove this on production.
            CreateUserRoles(service).Wait();
        }



        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Creating Admin Role
            var roleCheck = await roleManager.RoleExistsAsync(UserRole.Admin.ToString());
            if (!roleCheck)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin.ToString()));

                // Creating the admin account
                var user = new AppUser
                {
                    UserName = "AdminUser",
                    Email = "default@default.com"

                };
                const string password = "Admin";
                var createUser = await userManager.CreateAsync(user, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                }
            }
            // Creating PremiumUser Role
            roleCheck = await roleManager.RoleExistsAsync(UserRole.PremiumUser.ToString());
            if (!roleCheck)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.PremiumUser.ToString()));
            }
            // Creating RegularUser Role
            roleCheck = await roleManager.RoleExistsAsync(UserRole.RegularUser.ToString());
            if (!roleCheck)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.RegularUser.ToString()));
            }

        }
    }
}
