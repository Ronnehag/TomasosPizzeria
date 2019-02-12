using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Display(Name="Lägg till ingrediens")]
        public string NewIngredient { get; set; }
    }
}
