using AZR_RED_THREAD_DAL.Models.Data;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AZR_RED_THREAD_DAL.Services.RolePrivilegeDAServices
{
    public class RolePrivilegeDAServices : IRolePrivilegeDAServices
    {
        private readonly IDataContext _context;
        public RolePrivilegeDAServices(IDataContext context) { _context = context; }

        public async Task<IEnumerable<RolePrivilege>> GetRolePrivilegeByRoleIdAsync(int roleId)
        {
            return await _context.RolePrivileges
                .Include(rp => rp.Privilege)
                .Where(rp => rp.RoleId == roleId && rp.IsActive)
                .ToListAsync();
        }

        public async Task<RolePrivilege> AddRolePrivilegeAsync(RolePrivilege rp)
        {
            _context.RolePrivileges.Add(rp);
            await _context.SaveChangesAsync();
            return rp;
        }

        public async Task<bool> RemoveRolePrivilegeAsync(int roleId, int privilegeId)
        {
            var item = await _context.RolePrivileges.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PrivilegeId == privilegeId && rp.IsActive);
            if (item == null) return false;
            item.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RolePrivilegeExistsAsync(int roleId, int privilegeId)
        {
            return await _context.RolePrivileges.AnyAsync(rp => rp.RoleId == roleId && rp.PrivilegeId == privilegeId && rp.IsActive);
        }
    }
}
