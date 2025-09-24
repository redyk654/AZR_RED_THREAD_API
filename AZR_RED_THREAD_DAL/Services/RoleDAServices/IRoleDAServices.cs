using System.Threading.Tasks;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;

namespace AZR_RED_THREAD_DAL.Services.RoleDAServices
{
    public interface IRoleDAServices
    {
        Task<IEnumerable<Roles>> GetAllRolesAsync();
        Task<Roles?> GetRoleByIdAsync(int id);
        Task<Roles?> GetRoleByLabelAsync(string label);
        Task<Roles> CreateRoleAsync(Roles role);
        Task<Roles> UpdateRoleAsync(Roles role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
