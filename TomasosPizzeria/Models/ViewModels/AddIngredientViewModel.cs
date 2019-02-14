using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Models.ViewModels
{
    public class AddIngredientViewModel
    {
        public List<Produkt> Ingredients { get; set; }

        [Display(Name = "Skapa ingrediens")]
        [StringLength(50, ErrorMessage = "Namn får inte överstiga 50 tecken")]
        [RegularExpression("^[A-ZÅÄÖa-zåäöé ]{2,}$", ErrorMessage = "Namn får endast innehålla a-ö")]
        public string NewIngredient { get; set; }

        [Display(Name = "Välj ingrediens")]
        public string SelectedIngredient { get; set; }

        public List<Produkt> IngredientsList { get; set; } = new List<Produkt>();
    }
}
