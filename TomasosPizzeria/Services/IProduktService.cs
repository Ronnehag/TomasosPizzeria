using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    /// <summary>
    /// Produkt = Ingredients
    /// </summary>
    public interface IProduktService
    {
        Task<List<Produkt>> GetAllProduktsAsync();
    }
}
