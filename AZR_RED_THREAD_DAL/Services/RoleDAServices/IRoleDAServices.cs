using System.Threading.Tasks;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;

namespace AZR_RED_THREAD_DAL.Services.RoleDAServices
{
    public interface IRoleDAServices
    {
        Task<Roles?> GetByLabelAsync(string label);
        Task<Roles> CreateRoleAsync(Roles role);
    }
}
