using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUserService _service;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IUserService service, UserManager<AppUser> usermanager)
        {
            _service = service;
            _userManager = usermanager;
        }

        [Route("details/{username}")]
        public async Task<IActionResult> EditDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var kund = await _service.FindUserAsync(user.Id);
            if (kund != null)
            {
                var model = new KundEditDetailsViewModel
                {
                    Kund = kund,
                    UserName = user.UserName,

                };
                return View(model);
            }
            return BadRequest(new { Error = "User not found" });
        }

        [HttpPost]
        [Route("details/{username}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(KundEditDetailsViewModel model) // New kund data
        {
            var user = await _userManager.GetUserAsync(User); // Get the logged in User
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                return PartialView("_EditDetailsForm", model);
            }

            // Confirm the password is correct before changing details further
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
            {
                ModelState.AddModelError("Password", "Fel lösenord angett");
                return PartialView("_EditDetailsForm", model);
            }

            // Change password if the user has added a new password
            if (!string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                // Validate the change is confirmed
                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                if (result.Succeeded)
                {
                    ViewBag.PassWordChanged = "Lösenord har uppdaterats";
                }
            }

            var newKundDetails = await _service.UpdateUserAsync(model.Kund);
            model.Kund = newKundDetails;
            ViewBag.KundUpdated = "Din data är uppdaterad";

            return PartialView("_EditDetailsForm", model);
        }



    }
}