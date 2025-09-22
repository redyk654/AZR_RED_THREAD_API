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

        public async Task<PaginatedResult<ProjectDto>> GetPaginatedProjectsAsync(int page, int pageSize)
        {
            // Validation des paramètres
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 6;
            if (pageSize > 100) pageSize = 100; // Limite max

            var (projects, total) = await _projectDAServices.GetPaginatedProjectsAsync(page, pageSize);
            var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);

            return new PaginatedResult<ProjectDto>
            {
                Data = projectDtos,
                Total = total,
                Page = page,
                PageSize = pageSize
            };
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

        public async Task<ProjectDto> UpdateProjectAsync(UpdateProjectDto updateProjectDto)
        {
            // Vérification existence
            var existingProject = await _projectDAServices.GetProjectByIdAsync(updateProjectDto.Id);
            if (existingProject == null)
            {
                throw new InvalidOperationException($"Le projet avec l'ID {updateProjectDto.Id} n'existe pas.");
            }

            // Validation métier : nom unique (exclure le projet actuel)
            if (await _projectDAServices.ProjectNameExistsAsync(updateProjectDto.Name, updateProjectDto.Id))
            {
                throw new InvalidOperationException($"Un autre projet avec le nom '{updateProjectDto.Name}' existe déjà.");
            }

            // Validation métier : dates cohérentes
            if (updateProjectDto.StartDate >= updateProjectDto.EndDate)
            {
                throw new InvalidOperationException("La date de fin doit être postérieure à la date de début.");
            }

            // Mise à jour des propriétés
            _mapper.Map(updateProjectDto, existingProject);

            var updatedProject = await _projectDAServices.UpdateProjectAsync(existingProject);
            return _mapper.Map<ProjectDto>(updatedProject);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            if (id <= 0) return false;

            // Vérification existence
            if (!await _projectDAServices.ProjectExistsAsync(id))
            {
                throw new InvalidOperationException($"Le projet avec l'ID {id} n'existe pas.");
            }

            // TODO: Vérifier si le projet a des tâches associées
            // Dans ce cas, on pourrait empêcher la suppression ou supprimer en cascade

            return await _projectDAServices.DeleteProjectAsync(id);
        }
    }
}
