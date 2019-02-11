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
        public Kund Kund { get; set; }
        public bool CalculatedPoints { get; set; }

        private bool HasPointsForFreePizza()
        {
            var value = Kund.Bonuspoäng >= 100 && Products.Count >= 1;
            if (value)
            {
                CalculatedPoints = true;
            }
            else
            {
                CalculatedPoints = false;
            }

            return value;
        }

        /// <summary>
        /// Gets the discount amount, used for display purposes in the view model.
        /// </summary>
        public int DiscountAmount()
        {
            var sum = TotalSum();
            return (int) Math.Round(sum * 0.20, MidpointRounding.ToEven);
        }

        /// <summary>
        /// Gets the total sum for the cart, adds the discount if the user is in premium role.
        /// </summary>
        public int TotalSum(UserRole role)
        {
            var sum = TotalSum();
            if (role == UserRole.PremiumUser)
            {
                if (HasPointsForFreePizza())
                {
                    sum -= Products[0].Pris;
                }
                sum -= (int) Math.Round(sum * 0.20, MidpointRounding.ToEven);
            }
            return sum;
        }
        
        /// <summary>
        /// Gets the total sum in the cart for regular users.
        /// </summary>
        public int TotalSum()
        {
            int sum = 0;
            foreach (var product in Products)
            {
                sum += product.Pris;
            }
            return sum;
        }

        /// <summary>
        /// Groups the items in the list as new CartItemViewModels.
        /// </summary>
        public List<CartItemViewModel> GroupItems()
        {
            var query = Products
                .GroupBy(p => p.MatrattNamn)
                .Select(g => new CartItemViewModel
                {
                    ProductName = g.First().MatrattNamn,
                    Id = g.First().MatrattId,
                    Count = g.Count(),
                    TotalSum = g.Sum(d => d.Pris),
                }).ToList();
            return query;
        }

    }


}
