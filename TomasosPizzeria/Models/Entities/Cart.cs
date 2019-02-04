using System;
using System.Collections.Generic;

namespace TomasosPizzeria.Models.Entities
{
    public partial class Cart
    {
        public Cart()
        {
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int CartId { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}
