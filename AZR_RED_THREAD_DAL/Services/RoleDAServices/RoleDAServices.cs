using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using AZR_RED_THREAD_DAL.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Services.RoleDAServices
{
    public class RoleDAServices : IRoleDAServices
    {
        private readonly IDataContext _context;

        public RoleDAServices(IDataContext context)
        {
            _context = context;
        }

        public async Task<Roles?> GetByLabelAsync(string label)
        {
            if (string.IsNullOrWhiteSpace(label)) return null;
            return await _context.Roles.FirstOrDefaultAsync(r => r.IsActive && r.Label.ToLower() == label.ToLower());
        }

        public async Task<Roles> CreateRoleAsync(Roles role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}
