using System.Collections.Generic;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public interface IDishService
    {
        Task<List<Matratt>> GetAllDishesAsync();
        Task<Matratt> GetDishAsync(int id);
        Task<List<Produkt>> GetDishIngredientsAsync();
        Task<List<MatrattTyp>> GetDishCategoriesAsync();
        Task<bool> AddIngredientToDish(string name, int matrattId);
    }
}
