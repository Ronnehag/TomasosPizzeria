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
        private readonly UserManager<AppUser> _usermanager;

        public AccountController(IUserService service, UserManager<AppUser> usermanager)
        {
            _service = service;
            _usermanager = usermanager;
        }

        [Route("details/{username}")]
        public async Task<IActionResult> EditDetails()
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null) return Challenge();

            var kund = await _service.FindUserAsync(user.Id);
            if (kund != null)
            {
                var model = new KundViewModel
                {
                    Kund = kund,
                    AnvandarNamn = user.UserName,

                };
                return View(model);
            }
            return BadRequest(new {Error = "User not found"});
        }

        [HttpPost]
        [Route("details/{username}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(KundViewModel model) // New kund data
        {
            var user = await _usermanager.GetUserAsync(User); // Get the logged in User
            if (user == null) return Challenge();

            if (ModelState.IsValid) // Validating the data from the KundViewModel
            {
                await _usermanager.RemovePasswordAsync(user); // Removes current password
                await _usermanager.AddPasswordAsync(user, model.Losenord); // Adds new password
                var result = await _service.UpdateUserAsync(model.Kund); // Update Kund information in database
                model.Kund = result; // Sets the updated Kund in the KundViewModel

                return View(model); // returns result
            }

            return View(model); // Returns current state if not valid
        }



    }
}