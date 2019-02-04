using System;
using System.Collections.Generic;

namespace TomasosPizzeria.Models.Entities
{
    public partial class ShoppingCart
    {
        public ShoppingCart()
        {
            CartItem = new HashSet<CartItem>();
        }

        public int CartId { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<CartItem> CartItem { get; set; }
    }
}
