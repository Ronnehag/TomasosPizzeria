using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Models.ViewModels;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IProduktService _produktService;

        public DishController(IDishService dishService, IProduktService produktService)
        {
            _dishService = dishService;
            _produktService = produktService;
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

            // Check if any ingredients in the string
            string[] arr;
            if (!string.IsNullOrWhiteSpace(model.IngrediensNotInList))
            {
                var produkts = await _dishService.GetDishIngredientsAsync();
                arr = model.IngrediensNotInList.Split(" ");
                foreach (var str in arr)
                {
                    // Check if name doesn't exist already
                    if (!produkts.Any(p => string.Equals(p.ProduktNamn, str, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        // Create the product, add it to the database and return the ID. Append the ID to the selected products.
                        var produkt = new Produkt { ProduktNamn = str };
                        var newId = await _produktService.AddNewProdukt(produkt);
                        model.SelectedIngredients.Add(newId);
                    }
                    else
                    {
                        // If name exists, get the ID with that name and append it to the list instead.
                        var produktId = await _produktService.GetProduktIdByName(str);
                        model.SelectedIngredients.Add(produktId);
                    }
                }
            }

            // Link the Products with the connection table
            var matrattProdukter = new List<MatrattProdukt>();

            // Creating the new Dish and store it to the DB.
            var dish = new Matratt
            {
                MatrattNamn = model.Name,
                Beskrivning = model.Description,
                MatrattTyp = model.FoodType,
                Pris = model.Price,
            };
            dish = _dishService.AddNewDish(dish);

            // Remove duplicates incase there is one, fail safe.
            foreach (var produktId in model.SelectedIngredients.Distinct())
            {
                matrattProdukter.Add(new MatrattProdukt
                {
                    MatrattId = dish.MatrattId,
                    ProduktId = produktId
                });
            }

            // Add the list of MattrattProdukter to the property of the Matratt, save changes.
            dish.MatrattProdukt = matrattProdukter;

            await _dishService.UpdateDishAsync(dish);







            return RedirectToAction("AdminPage", "Admin");

        }

    }
}