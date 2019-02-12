using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> AddIngredient(EditDishViewModel mdl)
        {
            // Get the ID of the dish
            var matrattId = mdl.Dish.MatrattId;

            // Check if modelstate is valid

            // Loop the ingredients, attach them to the dish
            await _dishService.AddIngredientToDish(mdl.NewIngredient, matrattId);


            var dish = await _dishService.GetDishAsync(matrattId);
            var model = new EditDishViewModel
            {
                Dish = dish,
                NewIngredient = ""
            };

            return PartialView("_AddIngredientPartialView", model);
        }

        public IActionResult RemoveIngredient(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}