﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;
using TomasosPizzeria.Models.ViewModels;

namespace TomasosPizzeria.Services
{
    public interface IDishService
    {
        Task<List<Matratt>> GetAllDishesAsync();
        Task<Matratt> GetDishAsync(int id);
        Task<Matratt> GetOnlyDishAsync(int id);
        Task<List<Produkt>> GetDishIngredientsAsync();
        Task<List<MatrattTyp>> GetDishCategoriesAsync();
        Task<bool> UpdateDishAsync(Matratt dish);
        Task<bool> AddIngredientToDish(string name, int matrattId);
        Task<bool> RemoveIngredientFromDish(int produktId, int matrattId);
        Matratt AddNewDish(Matratt dish);
    }
}
