using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.Access;
using AZR_RED_THREAD_DAL.Services.RoleDAServices;
using AZR_RED_THREAD_DAL.Services.PrivilegeDAServices;
using AZR_RED_THREAD_DAL.Services.RolePrivilegeDAServices;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AZR_RED_THREAD_BLL.Services.Access
{
    public class RoleServices : IRoleServices
    {
        private readonly IRoleDAServices _roleDA;
        private readonly IPrivilegeDAServices _privDA;
        private readonly IRolePrivilegeDAServices _rpDA;
        private readonly IMapper _mapper;

        public RoleServices(IRoleDAServices roleDA, IPrivilegeDAServices privDA, IRolePrivilegeDAServices rpDA, IMapper mapper)
        {
            _roleDA = roleDA;
            _privDA = privDA;
            _rpDA = rpDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var entities = await _roleDA.GetAllRolesAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(entities);
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            var e = await _roleDA.GetRoleByIdAsync(id);
            return e == null ? null : _mapper.Map<RoleDto>(e);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto, int createdBy)
        {
            var entity = _mapper.Map<Roles>(dto);
            entity.CreatedAt = System.DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            entity.IsActive = true;
            var created = await _roleDA.CreateRoleAsync(entity);
            return _mapper.Map<RoleDto>(created);
        }

        public async Task<RoleDto> UpdateRoleAsync(UpdateRoleDto dto, int updatedBy)
        {
            var existing = await _roleDA.GetRoleByIdAsync(dto.Id);
            if (existing == null) throw new InvalidOperationException("Role not found");

            existing.Label = dto.Label;
            existing.Description = dto.Description;
            existing.UpdatedAt = System.DateTime.UtcNow;
            existing.UpdatedBy = updatedBy;

            var updated = await _roleDA.UpdateRoleAsync(existing);
            return _mapper.Map<RoleDto>(updated);
        }

        public async Task<bool> DeleteRoleAsync(int id, int performedBy)
        {
            // potential business checks (can't delete Admin role etc.)
            var existing = await _roleDA.GetRoleByIdAsync(id);
            if (existing == null) return false;
            if (existing.Label.ToLower() == "admin") throw new InvalidOperationException("Admin role cannot be deleted");
            return await _roleDA.DeleteRoleAsync(id);
        }

        // Role-privilege management
        public async Task<IEnumerable<PrivilegeDto>> GetPrivilegesForRoleAsync(int roleId)
        {
            var list = await _rpDA.GetRolePrivilegeByRoleIdAsync(roleId);
            var privs = list.Select(rp => rp.Privilege!).Where(p => p != null);
            return _mapper.Map<IEnumerable<PrivilegeDto>>(privs);
        }

        public async Task AddPrivilegeToRoleAsync(int roleId, int privilegeId, int performedBy)
        {
            // check role & privilege exist
            var role = await _roleDA.GetRoleByIdAsync(roleId) ?? throw new InvalidOperationException("Role not found");
            var priv = await _privDA.GetPrivilegeByIdAsync(privilegeId) ?? throw new InvalidOperationException("Privilege not found");

            if (await _rpDA.RolePrivilegeExistsAsync(roleId, privilegeId)) return; // idempotent
            var rp = new RolePrivilege
            {
                RoleId = roleId,
                PrivilegeId = privilegeId,
                CreatedAt = System.DateTime.UtcNow,
                CreatedBy = performedBy,
                IsActive = true
            };
            await _rpDA.AddRolePrivilegeAsync(rp);
        }

        public async Task RemovePrivilegeFromRoleAsync(int roleId, int privilegeId, int performedBy)
        {
            if (!await _rpDA.RolePrivilegeExistsAsync(roleId, privilegeId))
                throw new InvalidOperationException("Privilege not assigned to role");
            await _rpDA.RemoveRolePrivilegeAsync(roleId, privilegeId);
        }
    }
}
