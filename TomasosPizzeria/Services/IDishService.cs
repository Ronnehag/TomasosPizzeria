using System.Collections.Generic;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public interface IDishService
    {
        Task<List<Matratt>> GetAllDishesAsync();
    }
}
