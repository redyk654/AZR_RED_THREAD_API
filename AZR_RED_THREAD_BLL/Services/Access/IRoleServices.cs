using AZR_RED_THREAD_BLL.DTOs.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.Access
{
    public interface IRoleServices
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto dto, int createdBy);
        Task<RoleDto> UpdateRoleAsync(UpdateRoleDto dto, int updatedBy);
        Task<bool> DeleteRoleAsync(int id, int performedBy);
        // role-privileges
        Task<IEnumerable<PrivilegeDto>> GetPrivilegesForRoleAsync(int roleId);
        Task AddPrivilegeToRoleAsync(int roleId, int privilegeId, int performedBy);
        Task RemovePrivilegeFromRoleAsync(int roleId, int privilegeId, int performedBy);
    }
}
