using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.IdentityData;
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


        private IActionResult FillCustomerGrid()
        {
            var data = _userService.GetAll();
            return null;
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