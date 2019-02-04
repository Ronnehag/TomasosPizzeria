
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetCart(string userId);
    }
}
