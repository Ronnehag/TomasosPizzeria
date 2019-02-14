using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class NewDishViewModel
    {
        [Required(ErrorMessage = "Du måste ange ett namn")]
        [Display(Name = "Namn")]
        [StringLength(50, ErrorMessage = "Namnet får inte överstiga 50 bokstäver")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        [StringLength(200, ErrorMessage = "Beskrivningens längd får inte överstiga 200 bokstäver")]
        public string Description { get; set; }

        public List<Produkt> SelectedIngredientsList { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Värdet för {0} kan inte vara 0")]
        [Display(Name = "Pris")]
        public int Price { get; set; }

        [Display(Name = "Kategori")]
        [Range(1, int.MaxValue, ErrorMessage = "Du måste välja en kategori")]
        public int FoodType { get; set; }

        public SelectList FoodTypeSelectList { get; set; }

        public AddIngredientViewModel IngredientViewModel { get; set; }

    }
}
