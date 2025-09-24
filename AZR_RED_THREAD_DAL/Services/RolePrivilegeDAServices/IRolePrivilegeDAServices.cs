using System.Collections.Generic;
using System.Threading.Tasks;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;

namespace AZR_RED_THREAD_DAL.Services.RolePrivilegeDAServices
{
    public interface IRolePrivilegeDAServices
    {
        Task<IEnumerable<RolePrivilege>> GetRolePrivilegeByRoleIdAsync(int roleId);
        Task<RolePrivilege> AddRolePrivilegeAsync(RolePrivilege rp);
        Task<bool> RemoveRolePrivilegeAsync(int roleId, int privilegeId);
        Task<bool> RolePrivilegeExistsAsync(int roleId, int privilegeId);
    }
}
