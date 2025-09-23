using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.UserDto;
using AZR_RED_THREAD_DAL.Services.UserDAServices;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly IUserDAServices _userDA;
        private readonly IMapper _mapper;

        public UserServices(IUserDAServices userDA, IMapper mapper)
        {
            _userDA = userDA;
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
    }
}
