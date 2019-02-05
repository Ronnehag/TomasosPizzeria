using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasosPizzeria.Models.ViewModels
{
    public class CartItemViewModel
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
        public int TotalSum { get; set; }
    }
}
