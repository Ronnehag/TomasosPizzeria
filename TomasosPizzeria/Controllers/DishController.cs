using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dishService.GetDishCategoriesAsync();
            var ingredients = await _dishService.GetDishIngredientsAsync();

            var viewModel = new NewDishViewModel
            {
                SelectedIngredients = new List<int>(),
                Ingredients = new MultiSelectList(ingredients, "ProduktId", "ProduktNamn"),
                FoodTypeSelectList = new SelectList(categories, "MatrattTyp1", "Beskrivning")
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewDishViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _dishService.GetDishCategoriesAsync();
                var ingredients = await _dishService.GetDishIngredientsAsync();
                model.Ingredients = new MultiSelectList(ingredients, "ProduktId", "ProduktNamn");
                model.FoodTypeSelectList = new SelectList(categories, "MatrattTyp1", "Beskrivning");
                return View(model);
            }

            return null;

        }

    }
}