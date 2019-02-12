using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public class DishService : IDishService
    {
        private readonly TomasosContext _context;
        private readonly IProduktService _produktService;

        public DishService(TomasosContext context, IProduktService produktService)
        {
            _context = context;
            _produktService = produktService;
        }

        public async Task<List<Matratt>> GetAllDishesAsync()
        {
            return await _context.Matratt.Include(x => x.MatrattTypNavigation)
                 .Include(m => m.MatrattProdukt)
                 .ThenInclude(p => p.Produkt)
                .ToListAsync();
        }

        public async Task<Matratt> GetDishAsync(int id)
        {
            return await _context.Matratt
                .Include(m => m.MatrattProdukt)
                .ThenInclude(p => p.Produkt)
                .FirstOrDefaultAsync(m => m.MatrattId == id);
        }

        public async Task<List<Produkt>> GetDishIngredientsAsync()
        {
            return await _context.Produkt.ToListAsync();
        }

        public async Task<List<MatrattTyp>> GetDishCategoriesAsync()
        {
            return await _context.MatrattTyp.ToListAsync();
        }

        public async void AddIngredientToDish(string name, int matrattId)
        {
            // Get the dish for the current ID
            var dish = await GetDishAsync(matrattId);

            // Get all ingredients in DB
            var produkter = await _produktService.GetAllProduktsAsync();

            // Check if the name doesn't exists in the DB
            if (!produkter.Any(p => string.Equals(p.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase)))
            {

            }



            // Check if name exists in DB, else create it.
            // IF exists, attach that ingredient to the dish.
        }
    }
}
