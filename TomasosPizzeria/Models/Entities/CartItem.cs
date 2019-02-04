using System;
using System.Collections.Generic;

namespace TomasosPizzeria.Models.Entities
{
    public partial class CartItem
    {
        public int CartId { get; set; }
        public int MatrattId { get; set; }
        public int Antal { get; set; }
        public int CartItemId { get; set; }

        public virtual ShoppingCart Cart { get; set; }
        public virtual Matratt Matratt { get; set; }
    }
}
