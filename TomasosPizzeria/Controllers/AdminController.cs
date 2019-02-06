using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly IDishService _dishService;
        private readonly IOrderService _orderService;

        public AdminController(UserManager<AppUser> userManager, IUserService userService, IDishService dishService, IOrderService orderService)
        {
            _userManager = userManager;
            _userService = userService;
            _dishService = dishService;
            _orderService = orderService;
        }

        public IActionResult Customers()
        {
            return View(_userManager.Users.ToList());
        }


        [HttpPost]
        public IActionResult UpdateUser()
        {
            return Ok(new {val = "Hello"});
            // TODO Hämta user, ändra roll. Om Regular -> Premium annars tvärtom.
            // Returerar till Customers som visar alla Users
            return RedirectToAction("Customers");
        }




        [HttpPost]
        public IActionResult UpdateUser(string id)
        {
            return Ok(new { val = "Hello" });
            // TODO Hämta user, ändra roll. Om Regular -> Premium annars tvärtom.
            // Returerar till Customers som visar alla Users
            return RedirectToAction("Customers");
        }

        public IActionResult EditDetails()
        {
            // TODO admin setting dashboard, change password and email only
            throw new System.NotImplementedException();
        }
    }
}