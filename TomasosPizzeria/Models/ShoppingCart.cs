using System.Collections.Generic;
using System.Linq;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Models.ViewModels;

namespace TomasosPizzeria.Models
{
    public class ShoppingCart
    {
        public List<Matratt> Products { get; set; }

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


        public List<CartItemViewModel> GroupItems()
        {
            var q = Products
                .GroupBy(p => p.MatrattNamn)
                .Select(g => new CartItemViewModel
                {
                    ProductName = g.First().MatrattNamn,
                    Count = g.Count(),
                    TotalSum = g.Sum(d => d.Pris),
                }).ToList();

            return q;
        }

    }


}
