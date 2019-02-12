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

        public DishService(TomasosContext context)
        {
            _context = context;
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

        public async Task<bool> UpdateDishAsync(Matratt dish)
        {
            _context.Entry(dish).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<bool> AddIngredientToDish(string name, int matrattId)
        {
            // Get the current dish
            var dish = await GetDishAsync(matrattId);

            // Get all ingredients in DB           
            var produkter = await GetDishIngredientsAsync();
            MatrattProdukt matrattProdukt;

            // Check if the name doesn't already exists in the DB
            if (!produkter.Any(p => string.Equals(p.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                // Name doesn't exist, create it and return the new produkt.
                var newProdukt = new Produkt { ProduktNamn = name };
                _context.Add(newProdukt);
                _context.SaveChanges();

                // Attach tne new produkt to MatrattProdukt.
                matrattProdukt = new MatrattProdukt
                {
                    MatrattId = matrattId,
                    ProduktId = newProdukt.ProduktId
                };
                dish.MatrattProdukt.Add(matrattProdukt);
                _context.Entry(dish).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                return result == 1;
            }

            // Check if the dish doesn't have that produkt already
            if (!dish.MatrattProdukt.Any(p => string.Equals(p.Produkt.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                // Take the produkt from the list of existing produkts and attach it to the dish.
                matrattProdukt = new MatrattProdukt
                {
                    MatrattId = matrattId,
                    ProduktId = produkter.First(p =>
                        string.Equals(p.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase)).ProduktId
                };
                _context.MatrattProdukt.Add(matrattProdukt);
                _context.Entry(dish).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                return result == 1;
            }

            return false;
        }

        public async Task<bool> RemoveIngredientFromDish(int produktId, int matrattId)
        {
            var dish = await GetDishAsync(matrattId);

            dish.MatrattProdukt = dish.MatrattProdukt.Where(d => d.ProduktId != produktId).ToList();
            _context.Entry(dish).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }
    }
}
