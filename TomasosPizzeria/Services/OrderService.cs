using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
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
        /// <summary>
        /// Adds the order to the database. PremiumUser will get the calculated price based on the discount and bonus points.
        /// </summary>
        public async Task<Bestallning> AddOrderAsync(string userId, ShoppingCart cart, UserRole role)
        {
            var kund = await _context.Kund.FirstOrDefaultAsync(u => u.UserId == userId);
            if (kund == null) return null;

            if (role == UserRole.PremiumUser)
            {
                // instantiates the int if it's null, meaning this user hasn't had any points before.
                // Add the points to the Kund, 10 points per item in the cart.
                if (kund.Bonuspoäng == null) kund.Bonuspoäng = 0;
                kund.Bonuspoäng += (cart.Products.Count * 10);
            }

            // Group the products by ID and counting them into new BestallningMatratt objects
            var orders = cart.Products
                .GroupBy(p => p.MatrattId)
                .Select(g => new BestallningMatratt
                {
                    MatrattId = g.First().MatrattId,
                    Antal = g.Count()

                }).ToList();

            // Puts the order together
            var order = new Bestallning
            {
                BestallningDatum = DateTime.Now,
                KundId = kund.KundId,
                Levererad = false,
                Totalbelopp = role == UserRole.PremiumUser ? cart.TotalSum(UserRole.PremiumUser) : cart.TotalSum(),
                BestallningMatratt = orders
            };

            // Calculates the points and saves to database
            if (kund.Bonuspoäng >= 100) kund.Bonuspoäng -= 100;
            _context.Add(order);
            _context.Entry(kund).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Bestallning
                .Include(b => b.BestallningMatratt)
                .Include(b => b.Kund)
                .FirstOrDefaultAsync(x => x.KundId == kund.KundId);
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

        public async Task<bool> MarkOrderAsDeliveredAsync(int orderId)
        {
            var order = await _context.Bestallning.FirstOrDefaultAsync(o => o.BestallningId == orderId);
            if (order != null)
            {
                order.Levererad = true;
            }
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<Bestallning> GetOrderAsync(int orderId)
        {
            return await _context.Bestallning
                .Include(o => o.BestallningMatratt)
                .FirstOrDefaultAsync(o => o.BestallningId == orderId);
        }

        public async Task<bool> RemoveOrderAsync(int orderId)
        {
            var order = await this.GetOrderAsync(orderId);
            if (order == null || !order.Levererad) return false;
            foreach (var dish in order.BestallningMatratt)
            {
                _context.BestallningMatratt.Remove(dish);
            }

            _context.Bestallning.Remove(order);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }
    }
}
