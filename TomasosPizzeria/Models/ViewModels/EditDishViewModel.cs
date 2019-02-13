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
        [RegularExpression("(^[a-zåäöA-ZÅÄÖ](( [a-zA-ZåäöÅÄÖ]+)|([a-zA-ZåäöÅÄÖ]))*$)|(^[a-zA-ZåäöÅÄÖ]$)"
            , ErrorMessage = "Ingrediens kan endast innehålla a-ö och måste separeras med mellanslag")]
        [Required(ErrorMessage = "Du måste ange ett namn")]
        public string NewIngredient { get; set; }
    }
}
