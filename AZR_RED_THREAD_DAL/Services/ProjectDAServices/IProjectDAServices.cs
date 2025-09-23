using AZR_RED_THREAD_DAL.Models.Project;

namespace AZR_RED_THREAD_DAL.Services.ProjectDAServices
{
    public interface IProjectDAServices
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<(IEnumerable<Project> Projects, int Total)> GetPaginatedProjectsAsync(int page, int pageSize);
        Task<Project?> GetProjectByIdAsync(int id);
        Task<(IEnumerable<Project> Projects, int Total)> GetPaginatedProjectsByUserAsync(int page, int pageSize, int userId);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
        Task<bool> ProjectExistsAsync(int id);
        Task<bool> ProjectNameExistsAsync(string name, int? excludeId = null);
    }
}