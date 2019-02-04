using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IDishService _dishService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IShoppingCartService _cartService;


        public CartController(IDishService dishService, UserManager<AppUser> userManager, IShoppingCartService cartService)
        {
            _dishService = dishService;
            _userManager = userManager;
            _cartService = cartService;
        }

        public async Task<IActionResult> AddToCart(int productId)
        {


            return null;
        }
    }
}