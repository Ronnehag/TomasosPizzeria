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
        [RegularExpression("^[a-zåäöèA-ZÅÄÖ ]{2,}$", ErrorMessage = "En ingrediens får endast innehålla (a-ö och mellanslag)")]
        [StringLength(50, ErrorMessage = "Namn får inte överstiga 50 bokstäver")]
        public string NewIngredient { get; set; }
    }
}
