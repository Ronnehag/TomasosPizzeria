using Microsoft.EntityFrameworkCore;
using System;
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
            // Get the current dish
            var dish = await GetDishAsync(matrattId);

            // Get all ingredients in DB           
            var produkter = await _produktService.GetAllProduktsAsync();
            MatrattProdukt matrattProdukt = null;

            // Check if the name doesn't already exists in the DB
            if (!produkter.Any(p => string.Equals(p.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                // Name doesn't exist, create it and return the new produkt.
                var newProdukt = await _produktService.AddNewProduktAsync(name);
                // Attach tne new produkt to MatrattProdukt and add that to the database context.
                matrattProdukt = new MatrattProdukt
                {
                    MatrattId = matrattId,
                    ProduktId = newProdukt.ProduktId,
                    Produkt = newProdukt
                };
                
            }
            else
            {
                // Check if the dish doesn't have that produkt already
                if(!dish.MatrattProdukt.Any(p => string.Equals(p.Produkt.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    // Take the produkt from the list of existing produkts and attach it to the dish.
                    matrattProdukt = new MatrattProdukt
                    {
                        MatrattId = matrattId,
                        Produkt = produkter.First(p =>
                            string.Equals(p.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase))
                    };
                }
            }

            if (matrattProdukt == null) return;
            _context.Add(matrattProdukt);
            _context.SaveChanges();
        }
    }
}
