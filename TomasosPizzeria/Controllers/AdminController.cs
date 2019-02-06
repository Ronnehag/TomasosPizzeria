using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult AdminPage()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Customers()
        {
            return PartialView("_CustomersPartialView", _userManager.Users.ToList());
        }

        public async Task<IActionResult> UpdateUserRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            //Check current role, if role is regular change it to premium else the other way around.
            if (await _userManager.IsInRoleAsync(user, UserRole.RegularUser.ToString()))
            {
                await _userManager.RemoveFromRoleAsync(user, UserRole.RegularUser.ToString());
                await _userManager.AddToRoleAsync(user, UserRole.PremiumUser.ToString());
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, UserRole.PremiumUser.ToString());
                await _userManager.AddToRoleAsync(user, UserRole.RegularUser.ToString());
            }

            // Redirect to customers action to rerender the partial
            return RedirectToAction("Customers");
        }





        public IActionResult EditDetails()
        {
            // TODO admin setting dashboard, change password and email only
            throw new System.NotImplementedException();
        }
    }
}