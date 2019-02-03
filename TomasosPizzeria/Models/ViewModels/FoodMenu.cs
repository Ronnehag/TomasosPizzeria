using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class FoodMenu
    {
        public IEnumerable<Matratt> PizzaDishes { get; set; }
        public IEnumerable<Matratt> PastaDishes { get; set; }
        public IEnumerable<Matratt> SaladDishes { get; set; }
    }
}
