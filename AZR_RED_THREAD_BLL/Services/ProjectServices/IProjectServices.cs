using AZR_RED_THREAD_DAL.DTOs.CreateProjectDto;
using AZR_RED_THREAD_DAL.DTOs.PaginatedResult;
using AZR_RED_THREAD_DAL.DTOs.ProjectDto;
using AZR_RED_THREAD_DAL.DTOs.UpdateProjectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.ProjectServices
{
    public interface IProjectServices
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<PaginatedResult<ProjectDto>> GetPaginatedProjectsAsync(int page, int pageSize);
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<ProjectDto> UpdateProjectAsync(UpdateProjectDto updateProjectDto);
        Task<bool> DeleteProjectAsync(int id);
    }
}
