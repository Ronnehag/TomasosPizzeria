using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("dish/ingredient/add")]
        public IActionResult AddIngredient(EditDishViewModel mdl)
        {
            // Get the ID of the dish
            var matrattId = mdl.Dish.MatrattId;

            // Split the string by space, incase user puts in more than one ingredient.
            var ingredients = mdl.NewIngredient.Split(" ");

            // Loop the ingredients, attach them to the dish
            foreach (var ingredient in ingredients)
            {
                _dishService.AddIngredientToDish(ingredient, matrattId);
            }

            // Go to DB, check if it exists, append that to the Matratt, else create it and then append.
            throw new System.NotImplementedException();
        }

        public IActionResult RemoveIngredient(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}