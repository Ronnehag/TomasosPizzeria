using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
    }
}
