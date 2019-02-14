
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public interface IProduktService
    {
        Task<int> AddNewProdukt(Produkt prod);
        Task<int> GetProduktIdByName(string name);
        Task<Produkt> CreateProdukt(string name);
    }
}
