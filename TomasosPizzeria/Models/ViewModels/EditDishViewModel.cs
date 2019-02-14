using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class EditDishViewModel
    {
        public Matratt Dish { get; set; }

        [Display(Name = "Ingredienser")]
        public List<Produkt> Ingredients { get; set; }

        [Display(Name = "Kategori")]
        public List<MatrattTyp> Categories { get; set; }

        [Display(Name = "Ingrediens")]
        [RegularExpression("^[a-z¨åäöèA-Zåäö]{2,}", ErrorMessage = "En ingrediens får endast innehålla bokstäver")]
        public string NewIngredient { get; set; }
    }
}
