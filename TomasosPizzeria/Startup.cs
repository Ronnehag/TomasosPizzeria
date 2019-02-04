using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddMvc();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDishService, DishService>();

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
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
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

            CreateUserRoles(service).Wait(); // Creates the IdentityRoles
        }



        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Creating Admin Role
            var roleCheck = await roleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));

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
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Creating PremiumUser Role
            roleCheck = await roleManager.RoleExistsAsync("PremiumUser");
            if (!roleCheck)
            {
               
                await roleManager.CreateAsync(new IdentityRole("PremiumUser"));
            }

            // Creating RegularUser Role
            roleCheck = await roleManager.RoleExistsAsync("RegularUser");
            if (!roleCheck)
            {
                
                await roleManager.CreateAsync(new IdentityRole("RegularUser"));
            }

        }
    }
}
