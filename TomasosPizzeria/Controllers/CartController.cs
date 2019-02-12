using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;
using ShoppingCart = TomasosPizzeria.Models.ShoppingCart;

namespace TomasosPizzeria.Controllers
{
    [Authorize]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly IDishService _dishService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;


        public CartController(IDishService dishService, UserManager<AppUser> userManager, IOrderService orderService, IUserService userService)
        {
            _dishService = dishService;
            _userManager = userManager;
            _orderService = orderService;
            _userService = userService;
        }


        [Route("product/{id}")]
        public async Task<IActionResult> AddItem(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Challenge();

            ShoppingCart cart;

            var product = await _dishService.GetDishAsync(id);

            // If shopping cart doesn't exist
            if (HttpContext.Session.GetString("varukorg") == null)
            {
                cart = new ShoppingCart
                {
                    Products = new List<Matratt>(),
                    User = user,
                    Kund = await _userService.FindUserAsync(user.Id)
                };
            }
            else
            {
                // Else get the session and cast it as cart
                var serializedValue = ( HttpContext.Session.GetString("varukorg") );
                cart = JsonConvert.DeserializeObject<ShoppingCart>(serializedValue);
            }
            cart.Products.Add(product);

            //Lägga tillbaka listan i sessionsvariabeln
            var temp = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("varukorg", temp);
            return PartialView("_CartList", cart);
        }

        [Route("confirm")]
        public async Task<IActionResult> CheckOutPage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Challenge();
            var serializedValue = ( HttpContext.Session.GetString("varukorg") );

            var model = new CheckOutViewModel
            {
                Kund = await _userService.FindUserAsync(user.Id),
                Cart = JsonConvert.DeserializeObject<ShoppingCart>(serializedValue),
                UsePoints = false
            };
            return View(model);
        }

        [Route("checkout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(CheckOutViewModel mod)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Challenge();

            var serializedValue = ( HttpContext.Session.GetString("varukorg") );
            var model = JsonConvert.DeserializeObject<ShoppingCart>(serializedValue);

            Bestallning bestallning;

            // Check users Role
            if (await _userManager.IsInRoleAsync(user, UserRole.RegularUser.ToString()))
            {
                bestallning = await _orderService.AddOrderAsync(user.Id, model, UserRole.RegularUser);
            }
            else
            {
                bestallning = await _orderService.AddOrderAsync(user.Id, model, UserRole.PremiumUser);

            }
            // Resetting cart to 0 items
            model.Products = new List<Matratt>();
            var temp = JsonConvert.SerializeObject(model);
            HttpContext.Session.SetString("varukorg", temp);

            return View(bestallning);
        }

        [HttpGet]
        public IActionResult RemoveItem(int id)
        {
            var serializedValue = ( HttpContext.Session.GetString("varukorg") );
            var cart = JsonConvert.DeserializeObject<ShoppingCart>(serializedValue);
            cart.Products.Remove(cart.Products.First(x => x.MatrattId == id));

            var temp = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("varukorg", temp);

            return PartialView("_CartList", cart);
        }
    }
}