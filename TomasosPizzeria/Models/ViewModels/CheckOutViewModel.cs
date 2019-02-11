using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class CheckOutViewModel
    {
        public Kund Kund { get; set; }
        public ShoppingCart Cart { get; set; }
        public bool UsePoints { get; set; }
        public int TotalPrice { get; set; }
    }
}
