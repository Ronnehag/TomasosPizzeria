using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Bestallning> AddOrderAsync(string userId, ShoppingCart cart)
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

            return await _context.Bestallning
                .Include(b => b.BestallningMatratt)
                .Include(b => b.Kund).FirstOrDefaultAsync(x => x.KundId == kund.KundId);
        }

        public IEnumerable<Bestallning> GetAllOrders()
        {
            return _context.Bestallning.Include("Kund")
                .Include(x => x.BestallningMatratt).ThenInclude(o => o.Matratt)
                .AsEnumerable();
        }

        public async Task<Bestallning> GetOrderedDishesAsync(int id)
        {
            return await _context.Bestallning
                .Include(o => o.BestallningMatratt).ThenInclude(x => x.Matratt)
                .FirstOrDefaultAsync(o => o.BestallningId == id);
        }
    }
}
