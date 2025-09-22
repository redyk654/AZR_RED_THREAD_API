using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = AZR_RED_THREAD_DAL.Models.Task.Task;

namespace AZR_RED_THREAD_DAL.Services.TaskDAServices
{
    /// <summary>
    /// Contrat d'accès aux données pour les tâches.
    /// Fournit les opérations CRUD et utilitaires (pagination, existence, unicité label).
    /// </summary>
    public interface ITaskDAServices
    {
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<(IEnumerable<TaskEntity> Tasks, int Total)> GetPaginatedTasksAsync(int page, int pageSize);
        Task<TaskEntity?> GetTaskByIdAsync(int id);
        Task<TaskEntity> CreateTaskAsync(TaskEntity task);
        Task<TaskEntity> UpdateTaskAsync(TaskEntity task);
        Task<bool> DeleteTaskAsync(int id); // soft delete
        Task<bool> TaskExistsAsync(int id);
        Task<bool> TaskLabelExistsAsync(string label, int projectId, int? excludeId = null);
    }
}
