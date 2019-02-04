using System.Collections.Generic;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models
{
    public class ShoppingCart
    {
        public List<Matratt> Products { get; set; }
        public string UserId { get; set; }
    }
}
