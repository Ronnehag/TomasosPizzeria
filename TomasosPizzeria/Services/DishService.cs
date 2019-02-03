﻿using Microsoft.EntityFrameworkCore;
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
            return await _context.Matratt
                 .Include(m => m.MatrattProdukt)
                 .ThenInclude(p => p.Produkt).ToListAsync();
        }
    }
}
