using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class NewDishViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Ingrediens inte i listan? Ange med fri text för att skapa nya (separera med mellanslag)")]
        public string IngrediensNotInList { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int FoodType { get; set; }

        [Required]
        [Display(Name = "Ingredienser (välj flera, håll ned ctrl)")]
        public List<int> SelectedIngredients { get; set; }


        public MultiSelectList Ingredients { get; set; }

        public SelectList FoodTypeSelectList { get; set; }

    }
}
