using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [Route("editdetails")]
        public async Task<IActionResult> EditDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Challenge();

            var model = new AdminChangeDetailsViewModel
            {
                Username = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [Route("editdetails")]
        public async Task<IActionResult> EditDetails(AdminChangeDetailsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Challenge();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Validate that the password matches
            var correctPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!correctPassword)
            {
                ModelState.AddModelError("Password", "Fel lösenord angett");
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                // Change password
                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                if (result.Succeeded)
                {
                    ViewBag.PassWordChanged = "Lösenord har uppdaterats";
                }
            }
            // Check if user has changed email
            var confirmedEmail = await _userManager.GetEmailAsync(user);
            if (string.Equals(confirmedEmail, model.Email, StringComparison.CurrentCultureIgnoreCase))
            {
                return View(model);
            }

            // Change email
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
            var mail = await _userManager.ChangeEmailAsync(user, model.Email, token);
            if (mail.Succeeded)
            {
                ViewBag.EmailChanged = "Email har uppdaterats";
            }
            return View(model);
        }


        /* DISH SECTION */
        [Route("dish")]
        public async Task<IActionResult> GetTotalDishInformation(int id)
        {
            var model = await _dishService.GetDishAsync(id);
            return PartialView("_ProductModal", model);
        }

        [HttpPost("dish/edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeDishValues(EditDishViewModel mdl)
        {

            if (ModelState.IsValid)
            {
                await _dishService.UpdateDishAsync(mdl.Dish);
                return RedirectToAction("AdminPage");
            }

            // Else return new ViewModel
            mdl.Dish = await _dishService.GetDishAsync(mdl.Dish.MatrattId);
            mdl.Categories = await _dishService.GetDishCategoriesAsync();
            mdl.Ingredients = await _dishService.GetDishIngredientsAsync();
            return View("EditDish", mdl);
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
        [Route("dish/edit")]
        public async Task<IActionResult> AddIngredient(EditDishViewModel mdl)
        {
            // Check if the state is valid, see EditDishViewModel for valid attributes
            if (!ModelState.IsValid)
            {
                mdl.Dish = await _dishService.GetDishAsync(mdl.Dish.MatrattId);
                mdl.Categories = await _dishService.GetDishCategoriesAsync();
                mdl.Ingredients = await _dishService.GetDishIngredientsAsync();
                return PartialView("_AddIngredientPartialView", mdl);
            }

            // Save new ingredient and attach it to the dish
            await _dishService.AddIngredientToDish(mdl.NewIngredient, mdl.Dish.MatrattId);

            // Refill the ViewModel and return
            mdl.Dish = await _dishService.GetDishAsync(mdl.Dish.MatrattId);
            mdl.NewIngredient = string.Empty;
            mdl.Categories = await _dishService.GetDishCategoriesAsync();
            mdl.Ingredients = await _dishService.GetDishIngredientsAsync();
            return PartialView("_AddIngredientPartialView", mdl);
        }

        [Route("dish/edit")]
        public async Task<IActionResult> RemoveIngredient(string id)
        {
            var arr = id.Split("-");
            var produktId = int.Parse(arr[0]);
            var matrattId = int.Parse(arr[1]);

            await _dishService.RemoveIngredientFromDish(produktId, matrattId);

            // Refill the ViewModel and return
            var mdl = new EditDishViewModel
            {
                Dish = await _dishService.GetDishAsync(matrattId),
                NewIngredient = string.Empty,
                Categories = await _dishService.GetDishCategoriesAsync(),
                Ingredients = await _dishService.GetDishIngredientsAsync()
            };

            return PartialView("_AddIngredientPartialView", mdl);
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