using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Helpers;
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
            if (!string.IsNullOrWhiteSpace(model.IngrediensNotInList))
            {
                var produkts = await _dishService.GetDishIngredientsAsync();
                var arr = model.IngrediensNotInList.Split(" ");
                foreach (var str in arr)
                {
                    // Check if name doesn't exist already
                    if (!produkts.Any(p => string.Equals(p.ProduktNamn, str, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var toLower = str.ToLower();
                        var name = toLower.First().ToString().ToUpper() + toLower.Substring(1);
                        // Create the product, add it to the database and return the ID. Append the ID to the selected products.
                        var produkt = new Produkt { ProduktNamn = name };
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
            // Creating the new Dish and store it to the DB.
            var dish = new Matratt
            {
                MatrattNamn = model.Name.ToFirstLetterUpper(),
                Beskrivning = model.Description.ToFirstLetterUpper(),
                MatrattTyp = model.FoodType,
                Pris = model.Price,
            };
            dish = _dishService.AddNewDish(dish);

            // Remove duplicates from model incase there is one (fail safe).
            var matrattProdukter = model.SelectedIngredients
                .Distinct()
                .Select(produktId => new MatrattProdukt { MatrattId = dish.MatrattId, ProduktId = produktId }).ToList();

            // Add the list of MattrattProdukter to the property of the Matratt, save changes.
            dish.MatrattProdukt = matrattProdukter;

            await _dishService.UpdateDishAsync(dish);
            return RedirectToAction("AdminPage", "Admin");

        }

        public IActionResult RemoveIngredient(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> AddIngredient(NewDishViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.IngrediensNotInList))
            {
                var produkt = await _produktService.CreateProdukt(vm.IngrediensNotInList);

                // Kolla i DB om inte finns OM Finns returera den, ANNARS skapa ny och returera Produkt, adda till VM produkt lista, återskapa och returera viewmodel
                // Till partial
            }

            // Else add model error och returera

            return null;
        }
    }
}