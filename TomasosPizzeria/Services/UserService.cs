using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Services
{
    public class UserService : IUserService
    {
        private readonly TomasosContext _context;

        public UserService(TomasosContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds Kund to database, returns boolean if succeed
        /// </summary>
        public async Task<bool> AddUserAsync(Kund kund) //TODO kan kontrollera om mailen/användarnamnet är unika
        {
            _context.Kund.Add(kund);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        /// <summary>
        /// Finds the Kund from database using the core identity user ID.
        /// </summary>
        public async Task<Kund> FindUserAsync(string id)
        {
            return await _context.Kund.FirstOrDefaultAsync(k => k.UserId == id);

        }

        public async Task<Kund> UpdateUserAsync(Kund kund)
        {
            _context.Entry(kund).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return kund;
        }
    }
}
