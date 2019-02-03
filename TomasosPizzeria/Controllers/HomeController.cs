using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDishService _dishService;

        public HomeController(IDishService service)
        {
            _dishService = service;
        }

        public async Task<IActionResult> Index()
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