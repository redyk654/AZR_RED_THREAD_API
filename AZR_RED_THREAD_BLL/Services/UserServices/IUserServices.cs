using System.Threading.Tasks;
using AZR_RED_THREAD_BLL.DTOs.UserDto;

namespace AZR_RED_THREAD_BLL.Services.UserServices
{
    public interface IUserServices
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByM365UUIDAsync(string uuid);
        Task<bool> IsUserAdminAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task AssignRoleToUserAsync(int userId, int roleId, int performedBy);
        Task<UserDto> UpdateUserAsync(UserDto dto, int performedBy);
    }
}
