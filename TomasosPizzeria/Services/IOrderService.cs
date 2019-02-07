using System.Collections.Generic;
using System.Threading.Tasks;
using TomasosPizzeria.Models;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public interface IOrderService
    {
        Task<Bestallning> AddOrderAsync(string userId, ShoppingCart cart);
        IEnumerable<Bestallning> GetAllOrders();
        Task<Bestallning> GetOrderedDishesAsync(int orderId);
        Task<bool> MarkOrderAsDeliveredAsync(int orderId);
    }
}
