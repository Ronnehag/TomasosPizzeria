using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;


/* User controller handles registration, login and log out. */

namespace TomasosPizzeria.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(IUserService service, UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager)
        {
            _userService = service;
            _userManager = usermanager;
            _signInManager = signInManager;
        }

        // Register Page
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        // Login Page
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username); // Gets user
                if (user != null)
                {
                    await _signInManager.SignOutAsync(); // Signs out current user (if any)
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false); // Signs in the new user

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                // If error with sign in (most likely wrong username or password)
                ModelState.AddModelError(nameof(LoginViewModel.Username), "Felaktigt användarnamn eller lösenord");
                ModelState.AddModelError(nameof(LoginViewModel.Password), ""); // Just for visibility in the View
            }
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(KundViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdentity = new AppUser { UserName = model.AnvandarNamn, Email = model.Kund.Email }; // New user
            var result = await _userManager.CreateAsync(userIdentity, model.Losenord); // Create core identity user
            if (result.Succeeded)
            {
                model.Kund.UserId = userIdentity.Id; // Link Core Identity UserId with Kund
                var success = await _userService.AddUserAsync(model.Kund); // Add Kund to SQL DB
                if (!success)
                {
                    return BadRequest(new { Error = "Could not add user to database" });
                }
                await _userManager.AddToRoleAsync(userIdentity, UserRole.RegularUser.ToString()); // Sets the role to RegularUser
                await _signInManager.SignInAsync(userIdentity, false); //Sign in after registration
                return RedirectToAction("Index", "Home");
            }

            // If fail, print errors to model
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }
    }
}