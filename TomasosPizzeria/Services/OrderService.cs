using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TomasosPizzeria.Models;
using TomasosPizzeria.Models.Entities;

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
            if (kund == null) return null;

            // Group the products by ID and counting them into new BestallningMatratt objects
            var orders = cart.Products
                .GroupBy(p => p.MatrattId)
                .Select(g => new BestallningMatratt
                {
                    MatrattId = g.First().MatrattId,
                    Antal = g.Count()
                }).ToList();

            var order = new Bestallning
            {
                BestallningDatum = DateTime.Now,
                KundId = kund.KundId,
                Levererad = false,
                Totalbelopp = cart.TotalSum(),
                BestallningMatratt = orders
            };
            _context.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
