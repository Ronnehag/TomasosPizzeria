using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.IdentityData;

namespace TomasosPizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        [HttpPost]
        public IActionResult UpdateUser(string id)
        {
            // TODO Hämta user, ändra roll. Om Regular -> Premium annars tvärtom.
           
            // Returerar till Index som visar alla Users
            return RedirectToAction("Index");
        }

        public IActionResult EditDetails()
        {
            // TODO admin setting dashboard, change password and email only
            throw new System.NotImplementedException();
        }
    }
}