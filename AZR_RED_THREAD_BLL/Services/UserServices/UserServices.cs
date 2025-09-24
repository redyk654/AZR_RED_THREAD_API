using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.UserDto;
using AZR_RED_THREAD_DAL.Services.UserDAServices;
using AZR_RED_THREAD_DAL.Services.RoleDAServices;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly IUserDAServices _userDA;
        private readonly IRoleDAServices _roleDA;
        private readonly IMapper _mapper;

        public UserServices(IUserDAServices userDA, IRoleDAServices roleDA, IMapper mapper)
        {
            _userDA = userDA;
            _roleDA = roleDA;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var u = await _userDA.GetByIdAsync(id);
            return u == null ? null : _mapper.Map<UserDto>(u);
        }

        public async Task<UserDto?> GetByM365UUIDAsync(string uuid)
        {
            var u = await _userDA.GetByM365UUIDAsync(uuid);
            return u == null ? null : _mapper.Map<UserDto>(u);
        }

        public async Task<bool> IsUserAdminAsync(int userId)
        {
            var u = await _userDA.GetByIdAsync(userId);
            if (u?.Role == null) return false;
            // Suppose role label "Admin" or "ROLE_ADMIN" indicates admin
            var label = u.Role.Label?.ToLower();
            return label == "admin" || label == "role_admin" || label == "administrator";
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userDA.GetAllUsersAsync();
            var dtos = users.Select(u =>
            {
                var dto = _mapper.Map<UserDto>(u);
                dto.RoleId = u.RoleId;
                dto.RoleLabel = u.Role?.Label;
                dto.IsActive = u.IsActive;
                return dto;
            }).ToList();

            return dtos;
        }

        // NEW: assigner role à un user
        public async Task AssignRoleToUserAsync(int userId, int roleId, int performedBy)
        {
            var user = await _userDA.GetByIdAsync(userId) ?? throw new InvalidOperationException("User not found");
            // Optionnel : valider le role existe
            var role = await _roleDA.GetRoleByIdAsync(roleId) ?? throw new InvalidOperationException("Role not found");

            user.RoleId = roleId;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = performedBy;

            await _userDA.UpdateUserAsync(user);
        }

        // Optional: mise à jour complète (utile si front envoie champs modifiables)
        public async Task<UserDto> UpdateUserAsync(UserDto dto, int performedBy)
        {
            var user = await _userDA.GetByIdAsync(dto.Id) ?? throw new InvalidOperationException("User not found");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = performedBy;

            var updated = await _userDA.UpdateUserAsync(user);

            var result = _mapper.Map<UserDto>(updated);
            result.RoleId = updated.RoleId;
            result.RoleLabel = updated.Role?.Label;
            result.IsActive = updated.IsActive;
            return result;
        }
    }
}
