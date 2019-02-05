using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TomasosPizzeria.Models.Entities;
using ShoppingCart = TomasosPizzeria.Models.ShoppingCart;

namespace TomasosPizzeria.Services
{
    public class OrderService : IOrderService
    {
        private readonly TomasosContext _context;

        public OrderService(TomasosContext context)
        {
            _context = context;
        }

        public async Task<Bestallning> AddOrder(string userId, ShoppingCart cart)
        {
            var kund = await _context.Kund.FirstOrDefaultAsync(u => u.UserId == userId);

            //TODO bestallningmatratt ??
            var order = new Bestallning
            {
                BestallningDatum = DateTime.Now,
                KundId = kund.KundId,
                Levererad = false,
                Totalbelopp = cart.TotalSum(),
            };
            return null;

        }
    }
}
