using AZR_RED_THREAD_BLL.DTOs.Access;
using AZR_RED_THREAD_DAL.Services.PrivilegeDAServices;
using AutoMapper;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.Access
{
    public class PrivilegeServices : IPrivilegeServices
    {
        private readonly IPrivilegeDAServices _privDA;
        private readonly IMapper _mapper;

        public PrivilegeServices(IPrivilegeDAServices privDA, IMapper mapper)
        {
            _privDA = privDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrivilegeDto>> GetAllPrivilegesAsync()
        {
            var entities = await _privDA.GetAllPrivilegesAsync();
            return _mapper.Map<IEnumerable<PrivilegeDto>>(entities);
        }

        public async Task<PrivilegeDto?> GetPrivilegeByIdAsync(int id)
        {
            var e = await _privDA.GetPrivilegeByIdAsync(id);
            return e == null ? null : _mapper.Map<PrivilegeDto>(e);
        }

        public async Task<PrivilegeDto> CreatePrivilegeAsync(PrivilegeDto dto, int createdBy)
        {
            var entity = _mapper.Map<Privilege>(dto);
            entity.CreatedAt = System.DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            entity.IsActive = true;
            var created = await _privDA.CreatePrivilegeAsync(entity);
            return _mapper.Map<PrivilegeDto>(created);
        }

        public async Task<PrivilegeDto> UpdatePrivilegeAsync(PrivilegeDto dto, int updatedBy)
        {
            var existing = await _privDA.GetPrivilegeByIdAsync(dto.Id) ?? throw new InvalidOperationException("Privilege not found");
            existing.Label = dto.Label;
            existing.Description = dto.Description;
            existing.UpdatedAt = System.DateTime.UtcNow;
            existing.UpdatedBy = updatedBy;
            var updated = await _privDA.UpdatePrivilegeAsync(existing);
            return _mapper.Map<PrivilegeDto>(updated);
        }

        public async Task<bool> DeletePrivilegeAsync(int id, int performedBy)
        {
            return await _privDA.DeletePrivilegeAsync(id);
        }
    }

}
