using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.Entities;
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
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // Login Page
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(KundViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create new user
            var userIdentity = new AppUser { UserName = model.AnvandarNamn, Email = model.Kund.Email };
            var result = await _userManager.CreateAsync(userIdentity, model.Losenord);
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

        [AllowAnonymous]
        [Route("signin-facebook")]
        public IActionResult LoginFacebook(string returnUrl)
        {
            var redirectUrl = Url.Action("FacebookResponse", "User", new { ReturnUrl = "/" });
            var props = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", props);
        }

        [AllowAnonymous]
        [Route("facebook-response")]
        public async Task<IActionResult> FacebookResponse(string returnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else // create new kund
            {
                var user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    EmailConfirmed = true
                };
                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    var identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        var kund = new Kund
                        {
                            Email = user.Email,
                            Gatuadress = info.Principal.FindFirst(ClaimTypes.StreetAddress)?.Value ?? "TBA",
                            Namn = info.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "TBA",
                            Postnr = info.Principal.FindFirst(ClaimTypes.PostalCode)?.Value ?? "TBA",
                            Telefon = info.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "TBA",
                            UserId = user.Id,
                            Postort = info.Principal.FindFirst(ClaimTypes.StateOrProvince)?.Value ?? "TBA"
                        };
                        await _userService.AddUserAsync(kund);
                        await _signInManager.SignInAsync(user, false);
                        return Redirect(returnUrl);
                    }
                }
            }

            return Forbid();
        }
    }
}