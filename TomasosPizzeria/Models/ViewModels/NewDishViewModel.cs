﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomasosPizzeria.Models.ViewModels
{
    public class NewDishViewModel
    {
        [Required]
        [Display(Name = "Namn")]
        [StringLength(50, ErrorMessage = "Namnet får inte överstiga 50 bokstäver")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        [StringLength(200, ErrorMessage = "Beskrivningens längd får inte överstiga 200 bokstäver")]
        public string Description { get; set; }

        [Display(Name = "Ingrediens inte i listan? Ange med fri text för att skapa nya (separera med mellanslag)")]
        [RegularExpression("(^[a-zåäöA-ZÅÄÖ](( [a-zA-ZåäöÅÄÖ]+)|([a-zA-ZåäöÅÄÖ]))*$)|(^[a-zA-ZåäöÅÄÖ]$)"
            , ErrorMessage = "Du måste separerar varje ingrediens med ett mellanslag")]
        public string IngrediensNotInList { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Värdet för {0} kan inte vara 0")]
        [Display(Name = "Pris")]
        public int Price { get; set; }

        [Display(Name = "Kategori")]
        [Range(1, int.MaxValue, ErrorMessage = "Du måste välja en kategori")]
        public int FoodType { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Du måste ange minst en ingrediens")]
        [Display(Name = "Ingredienser (välj flera, håll ned ctrl)")]
        public List<int> SelectedIngredients { get; set; }

        public MultiSelectList Ingredients { get; set; }

        public SelectList FoodTypeSelectList { get; set; }

    }
}
