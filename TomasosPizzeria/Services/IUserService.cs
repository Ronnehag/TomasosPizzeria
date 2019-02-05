using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeria.Models;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public interface IUserService
    {
        Task<bool> AddUserAsync(Kund kund);
        Task<Kund> FindUserAsync(string id);
        Task<Kund> UpdateUserAsync(Kund kund);
        IQueryable<Kund> GetAll();
    }
}
