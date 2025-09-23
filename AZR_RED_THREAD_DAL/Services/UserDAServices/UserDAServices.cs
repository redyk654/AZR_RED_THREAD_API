using AZR_RED_THREAD_DAL.Models.Data;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Services.UserDAServices
{
    /// <summary>
    /// Implémentation simple des accès lecture pour User.
    /// </summary>
    public class UserDAServices : IUserDAServices
    {
        private readonly IDataContext _context;

        public UserDAServices(IDataContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }

        public async Task<User?> GetByM365UUIDAsync(string m365uuid)
        {
            if (string.IsNullOrWhiteSpace(m365uuid)) return null;
            return await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.IsActive && u.M365UUID == m365uuid);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.IsActive && u.Email == email);
        }
    }
}
