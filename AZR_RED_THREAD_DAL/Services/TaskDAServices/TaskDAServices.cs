using AZR_RED_THREAD_DAL.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskEntity = AZR_RED_THREAD_DAL.Models.Task.Task;

namespace AZR_RED_THREAD_DAL.Services.TaskDAServices
{
    /// <summary>
    /// Implémentation DAL pour les tâches.
    /// Interagit directement avec le DataContext (EF Core).
    /// </summary>
    public class TaskDAServices : ITaskDAServices
    {
        private readonly IDataContext _context;

        public TaskDAServices(IDataContext context)
        {
            _context = context;
        }

        // Retourne toutes les tâches actives triées par date de création descendant
        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        // Pagination simple
        public async Task<(IEnumerable<TaskEntity> Tasks, int Total)> GetPaginatedTasksAsync(int page, int pageSize)
        {
            var query = _context.Tasks.Where(t => t.IsActive);

            var total = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tasks, total);
        }

        // Récupère une tâche par Id (active)
        public async Task<TaskEntity?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Project) // inclure projet si besoin
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
        }

        // Crée une tâche
        public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        // Met à jour une tâche
        public async Task<TaskEntity> UpdateTaskAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        // Soft delete : IsActive = false
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            task.IsActive = false;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }

        // Vérifie existence
        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id && t.IsActive);
        }

        // Vérifie unicité du label dans le même projet (utile à la création / update)
        public async Task<bool> TaskLabelExistsAsync(string label, int projectId, int? excludeId = null)
        {
            var query = _context.Tasks.Where(t => t.IsActive && t.Label == label && t.ProjectId == projectId);

            if (excludeId.HasValue)
                query = query.Where(t => t.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
