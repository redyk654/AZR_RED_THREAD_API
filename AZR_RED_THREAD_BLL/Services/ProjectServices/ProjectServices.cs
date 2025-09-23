using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.CreateProjectDto;
using AZR_RED_THREAD_BLL.DTOs.PaginatedResult;
using AZR_RED_THREAD_BLL.DTOs.ProjectDto;
using AZR_RED_THREAD_BLL.DTOs.UpdateProjectDto;
using AZR_RED_THREAD_DAL.Models.Project;
using AZR_RED_THREAD_DAL.Services.ProjectDAServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.ProjectServices
{
    public class ProjectServices : IProjectServices
    {
        private readonly IProjectDAServices _projectDAServices;
        private readonly IMapper _mapper;

        public ProjectServices(IProjectDAServices projectDAServices, IMapper mapper)
        {
            _projectDAServices = projectDAServices;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectDAServices.GetAllProjectsAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<(IEnumerable<ProjectDto> Data, int Total)> GetPaginatedProjectsAsync(int page, int pageSize, int? currentUserId = null, bool isAdmin = false)
        {
            if (isAdmin)
            {
                var (entities, total) = await _projectDAServices.GetPaginatedProjectsAsync(page, pageSize);
                return (_mapper.Map<IEnumerable<ProjectDto>>(entities), total);
            }

            if (currentUserId.HasValue)
            {
                var (entities, total) = await _projectDAServices.GetPaginatedProjectsByUserAsync(page, pageSize, currentUserId.Value);
                return (_mapper.Map<IEnumerable<ProjectDto>>(entities), total);
            }

            // if no user provided and not admin -> return empty
            return (Enumerable.Empty<ProjectDto>(), 0);
        }
        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            if (id <= 0) return null;

            var project = await _projectDAServices.GetProjectByIdAsync(id);
            return project == null ? null : _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            // Validation métier : nom unique
            if (await _projectDAServices.ProjectNameExistsAsync(createProjectDto.Name))
            {
                throw new InvalidOperationException($"Un projet avec le nom '{createProjectDto.Name}' existe déjà.");
            }

            // Validation métier : dates cohérentes
            if (createProjectDto.StartDate > createProjectDto.EndDate)
            {
                throw new InvalidOperationException("La date de fin doit être postérieure à la date de début.");
            }

            var project = _mapper.Map<Project>(createProjectDto);
            var createdProject = await _projectDAServices.CreateProjectAsync(project);

            return _mapper.Map<ProjectDto>(createdProject);
        }

        public async Task<ProjectDto> UpdateProjectAsync(UpdateProjectDto dto, int currentUserId, bool isAdmin)
        {
            var existing = await _projectDAServices.GetProjectByIdAsync(dto.Id);
            if (existing == null) throw new InvalidOperationException("Projet introuvable.");

            // Only creator or admin can update
            if (!isAdmin && existing.CreatedBy != currentUserId)
                throw new InvalidOperationException("Vous n'êtes pas autorisé à modifier ce projet.");

            // business validations...
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = dto.UpdatedBy;

            var updated = await _projectDAServices.UpdateProjectAsync(existing);
            return _mapper.Map<ProjectDto>(updated);
        }

        public async Task<bool> DeleteProjectAsync(int id, int currentUserId, bool isAdmin)
        {
            var existing = await _projectDAServices.GetProjectByIdAsync(id);
            if (existing == null) return false;

            if (!isAdmin && existing.CreatedBy != currentUserId)
                throw new InvalidOperationException("Vous n'êtes pas autorisé à supprimer ce projet.");

            return await _projectDAServices.DeleteProjectAsync(id);
        }
    }
}
