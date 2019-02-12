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
            // Check string, split by spaces.
            // Go to DB, check if it exists, append that to the Matratt, else create it and then append.
            throw new System.NotImplementedException();
        }
    }
}