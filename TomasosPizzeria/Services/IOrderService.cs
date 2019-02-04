using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;
using ShoppingCart = TomasosPizzeria.Models.ShoppingCart;

namespace TomasosPizzeria.Services
{
    public interface IOrderService
    {
        Task<Bestallning> AddOrder(string userId, ShoppingCart cart);
    }
}
