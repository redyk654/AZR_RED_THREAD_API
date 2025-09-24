using AZR_RED_THREAD_BLL.DTOs.CreateProjectDto;
using AZR_RED_THREAD_BLL.DTOs.PaginatedResult;
using AZR_RED_THREAD_BLL.DTOs.ProjectDto;
using AZR_RED_THREAD_BLL.DTOs.UpdateProjectDto;
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
        Task<(IEnumerable<ProjectDto> Data, int Total)> GetPaginatedProjectsAsync(int page, int pageSize, int? currentUserId = null, bool isAdmin = false);
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> UpdateProjectAsync(UpdateProjectDto dto, int currentUserId, bool isAdmin);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto); // Creation uses dto.CreatedBy
        Task<bool> DeleteProjectAsync(int id, int currentUserId, bool isAdmin);

    }
}
