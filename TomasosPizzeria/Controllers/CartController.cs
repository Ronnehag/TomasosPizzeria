﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IOrderService _orderService;


        public CartController(IDishService dishService, UserManager<AppUser> userManager, IOrderService orderService)
        {
            _dishService = dishService;
            _userManager = userManager;
            _orderService = orderService;
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
                    User = user
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

        [Route("checkout")]
        public async Task<IActionResult> CheckOut()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Challenge();

            var serializedValue = ( HttpContext.Session.GetString("varukorg") );
            var model = JsonConvert.DeserializeObject<ShoppingCart>(serializedValue);

            Bestallning bestallning = null;

            // Check users Role
            if (await _userManager.IsInRoleAsync(user, UserRole.RegularUser.ToString()))
            {
                bestallning = await _orderService.AddOrderAsync(user?.Id, model);
            }

            // todo check premium user, addOrder

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