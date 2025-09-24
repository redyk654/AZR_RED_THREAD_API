using AZR_RED_THREAD_DAL.Models.Data;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AZR_RED_THREAD_DAL.Services.PrivilegeDAServices
{
    public class PrivilegeDAServices : IPrivilegeDAServices
    {
        private readonly IDataContext _context;
        public PrivilegeDAServices(IDataContext context) { _context = context; }

        public async Task<IEnumerable<Privilege>> GetAllPrivilegesAsync()
        {
            return await _context.Privileges.Where(p => p.IsActive).OrderBy(p => p.Label).ToListAsync();
        }

        public async Task<Privilege?> GetPrivilegeByIdAsync(int id)
        {
            return await _context.Privileges.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<Privilege> CreatePrivilegeAsync(Privilege p)
        {
            _context.Privileges.Add(p);
            await _context.SaveChangesAsync();
            return p;
        }

        public async Task<Privilege> UpdatePrivilegeAsync(Privilege p)
        {
            _context.Privileges.Update(p);
            await _context.SaveChangesAsync();
            return p;
        }

        public async Task<bool> DeletePrivilegeAsync(int id)
        {
            var p = await GetPrivilegeByIdAsync(id);
            if (p == null) return false;
            p.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
