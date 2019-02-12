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

        /// <summary>
        /// Takes in the name of the ingredient and adds it to the database. Returns the added ingredient, method is async.
        /// </summary>
        public async Task<Produkt> AddNewProduktAsync(string name)
        {
            var produkt = new Produkt
            {
                ProduktNamn = name
            };

            _context.Produkt.Add(produkt);
            _context.SaveChanges();

            return await _context.Produkt.FirstOrDefaultAsync(p => p.ProduktId == produkt.ProduktId);
        }
    }
}
