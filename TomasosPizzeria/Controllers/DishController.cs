using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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

            var viewModel = new NewDishViewModel
            {
                SelectedIngredientsList = new List<Produkt>(),
                FoodTypeSelectList = new SelectList(categories, "MatrattTyp1", "Beskrivning"),
                IngredientViewModel = new AddIngredientViewModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(NewDishViewModel model)
        {
            if (!ModelState.IsValid)
            {
                List<Produkt> produktList;
                if (HttpContext.Session.GetString("ingredients") == null)
                {
                    produktList = new List<Produkt>();
                }
                else
                {
                    var serializedValue = ( HttpContext.Session.GetString("ingredients") );
                    produktList = JsonConvert.DeserializeObject<List<Produkt>>(serializedValue);
                }
                model.IngredientViewModel = new AddIngredientViewModel {Ingredients = produktList};
                var categories = await _dishService.GetDishCategoriesAsync();
                model.FoodTypeSelectList = new SelectList(categories, "MatrattTyp1", "Beskrivning");
                return View(model);
            }

            // Check if any ingredients in the string
            if (!string.IsNullOrWhiteSpace(model.NewIngredient))
            {
                var produkts = await _dishService.GetDishIngredientsAsync();
                var arr = model.NewIngredient.Split(" ");
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
            // Hämta Session Listan, filtrera bort ID, returera session
            var serializedValue = ( HttpContext.Session.GetString("ingredients") );
            var produktList = JsonConvert.DeserializeObject<List<Produkt>>(serializedValue);
            produktList = produktList.Where(p => p.ProduktId != id).ToList();

            var temp = JsonConvert.SerializeObject(produktList);
            HttpContext.Session.SetString("ingredients", temp);
            var vm = new AddIngredientViewModel { Ingredients = produktList };

            return PartialView("_NewDishAddIngredient", vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddIngredient(AddIngredientViewModel vm)
        {
            List<Produkt> produktList;
            if (HttpContext.Session.GetString("ingredients") == null)
            {
                produktList = new List<Produkt>();
            }
            else
            {
                var serializedValue = ( HttpContext.Session.GetString("ingredients") );
                produktList = JsonConvert.DeserializeObject<List<Produkt>>(serializedValue);
            }
            if (!string.IsNullOrWhiteSpace(vm.NewIngredient) && ModelState.IsValid)
            {
                var produkt = await _produktService.CreateProdukt(vm.NewIngredient);

                produktList.Add(produkt);
                var temp = JsonConvert.SerializeObject(produktList);
                HttpContext.Session.SetString("ingredients", temp);
            }
            vm.Ingredients = produktList;

            return PartialView("_NewDishAddIngredient", vm);
        }
    }
}