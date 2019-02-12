using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public class ProduktService : IProduktService
    {
        private readonly TomasosContext _context;

        public ProduktService(TomasosContext context)
        {
            _context = context;
        }

        public async Task<List<Produkt>> GetAllProduktsAsync()
        {
            return await _context.Produkt.ToListAsync();
        }

        public Task<Produkt> AddNewProduktAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
