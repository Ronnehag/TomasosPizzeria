using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IDishService _dishService;

        public OrderController(IDishService dishService)
        {
            _dishService = dishService;
        }


        public async Task<IActionResult> Order()
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