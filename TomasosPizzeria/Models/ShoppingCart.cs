using System.Collections.Generic;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models
{
    public class ShoppingCart
    {
        public IEnumerable<CartItem> Products { get; set; }
        public string UserId { get; set; }
    }

    public class CartItem
    {
        public Matratt Produkt { get; set; }
        public int Quanity { get; set; }
    }
}
