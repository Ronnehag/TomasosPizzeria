﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;
using ShoppingCart = TomasosPizzeria.Models.ShoppingCart;

namespace TomasosPizzeria.Controllers
{
    [Authorize]
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IOrderService _orderService;

        public OrderController(IDishService dishService, IOrderService orderService)
        {
            _dishService = dishService;
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin")]
        [Route("removeorder")]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            var success = await _orderService.RemoveOrderAsync(id);
            if (!success) return Redirect(Request.Headers["referer"]);



            return PartialView("_OrderDishCardPartial"); //TODO check
        }


        public async Task<IActionResult> Products()
        {
            ShoppingCart cart;
            if (HttpContext.Session.GetString("varukorg") == null)
            {
                cart = new ShoppingCart
                {
                    Products = new List<Matratt>()
                };
            }
            else
            {
                var serializedValue = ( HttpContext.Session.GetString("varukorg") );
                cart = JsonConvert.DeserializeObject<ShoppingCart>(serializedValue);
            }

            var allDishes = await _dishService.GetAllDishesAsync();
            var model = new FoodMenu
            {
                PizzaDishes = allDishes.Where(d => d.MatrattTyp == 1),
                PastaDishes = allDishes.Where(d => d.MatrattTyp == 2),
                SaladDishes = allDishes.Where(d => d.MatrattTyp == 3),
                ShoppingCart = cart
            };

            return View(model);
        }


    }
}