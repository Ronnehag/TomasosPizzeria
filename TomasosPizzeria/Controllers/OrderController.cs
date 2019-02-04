using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _usermanager;

        public OrderController(IDishService dishService, IUserService userService, UserManager<AppUser> usermanager)
        {
            _dishService = dishService;
            _userService = userService;
            _usermanager = usermanager;
        }


        public async Task<IActionResult> Products()
        {
            var allDishes = await _dishService.GetAllDishesAsync();
            var model = new FoodMenu
            {
                PizzaDishes = allDishes.Where(d => d.MatrattTyp == 1),
                PastaDishes = allDishes.Where(d => d.MatrattTyp == 2),
                SaladDishes = allDishes.Where(d => d.MatrattTyp == 3)
            };
            return View(model);
        }


    }
}