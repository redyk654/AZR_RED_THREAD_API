using System.Collections.Generic;
using System.Threading.Tasks;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;

namespace AZR_RED_THREAD_DAL.Services.PrivilegeDAServices
{
    public interface IPrivilegeDAServices
    {
        Task<IEnumerable<Privilege>> GetAllPrivilegesAsync();
        Task<Privilege?> GetPrivilegeByIdAsync(int id);
        Task<Privilege> CreatePrivilegeAsync(Privilege p);
        Task<Privilege> UpdatePrivilegeAsync(Privilege p);
        Task<bool> DeletePrivilegeAsync(int id);
    }
}
