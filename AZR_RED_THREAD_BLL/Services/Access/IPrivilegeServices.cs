using AZR_RED_THREAD_BLL.DTOs.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.Access
{
    public interface IPrivilegeServices
    {
        Task<IEnumerable<PrivilegeDto>> GetAllPrivilegesAsync();
        Task<PrivilegeDto?> GetPrivilegeByIdAsync(int id);
        Task<PrivilegeDto> CreatePrivilegeAsync(PrivilegeDto dto, int createdBy);
        Task<PrivilegeDto> UpdatePrivilegeAsync(PrivilegeDto dto, int updatedBy);
        Task<bool> DeletePrivilegeAsync(int id, int performedBy);
    }
}
