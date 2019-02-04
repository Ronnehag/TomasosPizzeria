using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly TomasosContext _context;

        public ShoppingCartService(TomasosContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            return null;

        }
    }
}
