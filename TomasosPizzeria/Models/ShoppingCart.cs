using System;
using System.Collections.Generic;
using System.Linq;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Models.ViewModels;

namespace TomasosPizzeria.Models
{
    public class ShoppingCart
    {
        public List<Matratt> Products { get; set; }
        public AppUser User { get; set; }


        public int DiscountAmount()
        {
            int sum = 0;
            foreach (var product in Products)
            {
                sum += product.Pris;
            }
            return (int) Math.Round(sum * 0.20, MidpointRounding.ToEven);
        }

        public int DiscountSum()
        {
            int sum = 0;
            foreach (var product in Products)
            {
                sum += product.Pris;
            }
            var discount = (int) Math.Round(sum * 0.20, MidpointRounding.ToEven);

            return sum - discount;
        }


        public int TotalSum()
        {
            int sum = 0;
            foreach (var product in Products)
            {
                sum += product.Pris;
            }

            return sum;
        }

        public List<CartItemViewModel> GroupItems()
        {
            var q = Products
                .GroupBy(p => p.MatrattNamn)
                .Select(g => new CartItemViewModel
                {
                    ProductName = g.First().MatrattNamn,
                    Id = g.First().MatrattId,
                    Count = g.Count(),
                    TotalSum = g.Sum(d => d.Pris),
                }).ToList();

            return q;
        }

    }


}
