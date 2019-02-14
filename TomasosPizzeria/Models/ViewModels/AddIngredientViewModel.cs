using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class AddIngredientViewModel
    {
        public List<Produkt> Ingredients { get; set; }

        [Required]
        [Display(Name = "Ingrediens")]
        [StringLength(50, ErrorMessage = "Namn får inte överstiga 50 tecken")]
        [RegularExpression("^[A-ZÅÄÖa-zåäöé ]{2,}$", ErrorMessage = "Namn får endast innehålla a-ö")]
        public string NewIngredient { get; set; }
    }
}
