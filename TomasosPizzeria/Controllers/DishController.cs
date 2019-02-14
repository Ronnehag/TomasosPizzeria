using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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


            var categories = await _dishService.GetDishCategoriesAsync();

            var viewModel = new NewDishViewModel
            {
                SelectedIngredientsList = new List<Produkt>(),
                FoodTypeSelectList = new SelectList(categories, "MatrattTyp1", "Beskrivning"),
                IngredientViewModel = new AddIngredientViewModel(),
            };
            viewModel.IngredientViewModel.IngredientsList = await _dishService.GetDishIngredientsAsync();
            viewModel.IngredientViewModel.Ingredients = produktList;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(NewDishViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var serializedValue = ( HttpContext.Session.GetString("ingredients") );
                var produktList = JsonConvert.DeserializeObject<List<Produkt>>(serializedValue);

                model.IngredientViewModel = new AddIngredientViewModel { Ingredients = produktList };
                var categories = await _dishService.GetDishCategoriesAsync();
                model.FoodTypeSelectList = new SelectList(categories, "MatrattTyp1", "Beskrivning");
                model.IngredientViewModel.IngredientsList = await _dishService.GetDishIngredientsAsync();
                return View(model);
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

            // Get the selected ingredients from the session
            var value = ( HttpContext.Session.GetString("ingredients") );
            var ingredientsList = JsonConvert.DeserializeObject<List<Produkt>>(value);

            // Attach them to the dish
            var dishIngredients = new List<MatrattProdukt>();
            foreach (var ingredient in ingredientsList)
            {
                dishIngredients.Add(new MatrattProdukt
                {
                    MatrattId = dish.MatrattId,
                    ProduktId = ingredient.ProduktId
                });
            }
            // Add the list of MattrattProdukter to the property of the Matratt, save changes.
            dish.MatrattProdukt = dishIngredients;

            // Resetting session
            ingredientsList = new List<Produkt>();

            await _dishService.UpdateDishAsync(dish);
            return RedirectToAction("AdminPage", "Admin");

        }

        public async Task<IActionResult> RemoveIngredient(int id)
        {
            // Hämta Session Listan, filtrera bort ID, returera session
            var serializedValue = ( HttpContext.Session.GetString("ingredients") );
            var produktList = JsonConvert.DeserializeObject<List<Produkt>>(serializedValue);
            produktList = produktList.Where(p => p.ProduktId != id).ToList();

            var temp = JsonConvert.SerializeObject(produktList);
            HttpContext.Session.SetString("ingredients", temp);
            var vm = new AddIngredientViewModel
            {
                Ingredients = produktList,
                IngredientsList = await _dishService.GetDishIngredientsAsync()
            };

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
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(vm.NewIngredient))
                {
                    var produkt = await _produktService.CreateProdukt(vm.NewIngredient);
                    if (produktList.All(p => p.ProduktNamn != produkt.ProduktNamn))
                    {
                        produktList.Add(produkt);
                    }
                }
                else
                {
                    var selectedProdukt = await _produktService.CreateProdukt(vm.SelectedIngredient);
                    if (produktList.All(p => p.ProduktNamn != selectedProdukt.ProduktNamn))
                    {
                        produktList.Add(selectedProdukt);
                    }
                }
                var temp = JsonConvert.SerializeObject(produktList);
                HttpContext.Session.SetString("ingredients", temp);
            }
            vm.Ingredients = produktList;
            vm.IngredientsList = await _dishService.GetDishIngredientsAsync();
            return PartialView("_NewDishAddIngredient", vm);
        }
    }
}