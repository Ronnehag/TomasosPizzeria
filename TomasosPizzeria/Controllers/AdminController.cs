using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDishService _dishService;
        private readonly IOrderService _orderService;

        public AdminController(UserManager<AppUser> userManager, IDishService dishService, IOrderService orderService)
        {
            _userManager = userManager;
            _dishService = dishService;
            _orderService = orderService;
        }

        // Admin dashboard main page
        [Route("dashboard")]
        public IActionResult AdminPage()
        {
            return View();
        }


        [HttpGet]
        [Route("customers")]
        public IActionResult Customers()
        {
            return PartialView("_CustomersPartialView", _userManager.Users.ToList());
        }

        [Route("updateuserrole")]
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
            return PartialView("_CustomerTableRowPartialView", user);
        }


        /* ORDER SECTION OF THE ADMIN CONTROLLER */

        [Route("orders")]
        public IActionResult GetOrders()
        {
            var model = _orderService.GetAllOrders();
            return PartialView("_OrderTablePartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOrder(int id)
        {
            var success = await _orderService.MarkOrderAsDeliveredAsync(id);
            if (!success)
            {
                return RedirectToAction("GetOrders");
            }
            var order = await _orderService.GetOrderAsync(id);

            return PartialView("_OrderValidatedPartial", order);
        }

        public IActionResult EditDetails()
        {
            // TODO admin setting dashboard, change password and email only
            throw new System.NotImplementedException();
        }


        /* DISH SECTION */
        [Route("dish")]
        public async Task<IActionResult> GetTotalDishInformation(int id)
        {
            var model = await _dishService.GetDishAsync(id);
            return PartialView("_ProductModal", model);
        }

        [HttpGet]
        [Route("dish/edit/{id}")]
        public async Task<IActionResult> EditDish(int id)
        {
            var model = new EditDishViewModel
            {
                Dish = await _dishService.GetDishAsync(id),
                Categories = await _dishService.GetDishCategoriesAsync(),
                Ingredients = await _dishService.GetDishIngredientsAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("dish/edit/{id}")]
        public async Task<IActionResult> AddIngredient(EditDishViewModel mdl)
        {
            // Get the ID of the dish
            var matrattId = mdl.Dish.MatrattId;

            // Check if modelstate is valid

            // Loop the ingredients, attach them to the dish
            await _dishService.AddIngredientToDish(mdl.NewIngredient, matrattId);

            var dish = await _dishService.GetDishAsync(matrattId);
            var model = new EditDishViewModel
            {
                Dish = dish,
                NewIngredient = ""
            };

            return PartialView("_AddIngredientPartialView", model);
        }

        [Route("dish/edit")]
        public async Task<IActionResult> RemoveIngredient(string id)
        {
            var arr = id.Split("-");
            var produktId = int.Parse(arr[0]);
            var matrattId = int.Parse(arr[1]);

            await _dishService.RemoveIngredientFromDish(produktId, matrattId);

            var dish = await _dishService.GetDishAsync(matrattId);
            var model = new EditDishViewModel
            {
                Dish = dish,
                NewIngredient = ""
            };

            return PartialView("_AddIngredientPartialView", model);
        }



        [HttpGet]
        public async Task<IActionResult> GetDishData(int id)
        {
            var model = await _orderService.GetOrderedDishesAsync(id);
            return PartialView("_OrderDishDataPartialView", model);
        }

        [Route("dishes")]
        public async Task<IActionResult> GetFoodDishes()
        {
            var dishes = await _dishService.GetAllDishesAsync();

            return PartialView("_DishesTablePartialView", dishes
                .OrderBy(d => d.MatrattTyp)
                .ToList());
        }



    }
}