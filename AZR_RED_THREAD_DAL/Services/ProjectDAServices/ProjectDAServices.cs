using AZR_RED_THREAD_DAL.Models.Data;
using AZR_RED_THREAD_DAL.Models.Project;
using AZR_RED_THREAD_DAL.Services.ProjectDAServices;
using Microsoft.EntityFrameworkCore;

namespace AZR_RED_THREAD_DAL.Services.ProjectDAServices
{
    public class ProjectDAServices : IProjectDAServices
    {
        private readonly IDataContext _context;

        public ProjectDAServices(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Project> Projects, int Total)> GetPaginatedProjectsAsync(int page, int pageSize)
        {
            var query = _context.Projects.Where(p => p.IsActive);

            var total = await query.CountAsync();

            var projects = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (projects, total);
        }

        public async Task<(IEnumerable<Project> Projects, int Total)> GetPaginatedProjectsByUserAsync(int page, int pageSize, int userId)
        {
            var query = _context.Projects.Where(p => p.IsActive);

            var total = await query.CountAsync();

            var projects = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (projects, total);
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await GetProjectByIdAsync(id);
            if (project == null) return false;

            // Soft delete
            project.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProjectExistsAsync(int id)
        {
            return await _context.Projects
                .AnyAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<bool> ProjectNameExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.Projects.Where(p => p.IsActive && p.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}