using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.Entities;
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
        private readonly IUserService _userService;


        public CartController(IDishService dishService, UserManager<AppUser> userManager, IUserService userService)
        {
            _dishService = dishService;
            _userManager = userManager;
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
                    Products = new List<Matratt>()
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

            return RedirectToAction("Products", "Order");
        }

        public async Task<IActionResult> CheckOut()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Challenge();


        }

    }
}