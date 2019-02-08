using System.Collections.Generic;
using System.Threading.Tasks;
using TomasosPizzeria.IdentityData;
using TomasosPizzeria.Models;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public interface IOrderService
    {
        Task<Bestallning> AddOrderAsync(string userId, ShoppingCart cart, UserRole role);
        IEnumerable<Bestallning> GetAllOrders();
        Task<Bestallning> GetOrderedDishesAsync(int orderId);
        Task<bool> MarkOrderAsDeliveredAsync(int orderId);
        Task<Bestallning> GetOrderAsync(int orderid);
        Task<bool> RemoveOrderAsync(int orderId);
    }
}
