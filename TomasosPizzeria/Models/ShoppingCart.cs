using System.Collections.Generic;
using System.Linq;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models
{
    public class ShoppingCart
    {
        public List<Matratt> Products { get; set; }
        public string UserId { get; set; }

        public int TotalSum()
        {
            int sum = 0;
            foreach (var product in Products)
            {
                sum += product.Pris;
            }

            return sum;
        }

        public int CountProducts(int id)
        {
            return Products.Count(p => p.MatrattId == id);
        }
    }
}
