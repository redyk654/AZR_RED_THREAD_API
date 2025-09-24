using AZR_RED_THREAD_DAL.Models.Data;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AZR_RED_THREAD_DAL.Services.RoleDAServices
{
    public class RoleDAServices : IRoleDAServices
    {
        private readonly IDataContext _context;
        public RoleDAServices(IDataContext context) { _context = context; }

        public async Task<IEnumerable<Roles>> GetAllRolesAsync()
        {
            return await _context.Roles.Where(r => r.IsActive).OrderBy(r => r.Label).ToListAsync();
        }

        public async Task<Roles?> GetRoleByLabelAsync(string label)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Label == label && r.IsActive);
        }

        public async Task<Roles?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<Roles> CreateRoleAsync(Roles role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Roles> UpdateRoleAsync(Roles role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null) return false;
            role.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
