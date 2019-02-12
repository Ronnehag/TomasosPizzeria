using System;
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

        public async Task<int> AddNewProdukt(Produkt prod)
        {
            _context.Add(prod);
            await _context.SaveChangesAsync();
            return prod.ProduktId;
        }

        public async Task<int> GetProduktIdByName(string name)
        {
            var produkt = await _context.Produkt.FirstOrDefaultAsync(p =>
                string.Equals(p.ProduktNamn, name, StringComparison.CurrentCultureIgnoreCase));

            return produkt?.ProduktId ?? 0;
        }
    }
}
