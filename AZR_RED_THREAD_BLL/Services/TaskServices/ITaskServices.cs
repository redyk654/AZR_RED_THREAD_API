using System.Collections.Generic;
using System.Threading.Tasks;
using AZR_RED_THREAD_BLL.DTOs.TaskDto;

namespace AZR_RED_THREAD_BLL.Services.TaskServices
{
    public interface ITaskServices
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<(IEnumerable<TaskDto> Data, int Total)> GetPaginatedTasksAsync(int page, int pageSize);
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
        Task<TaskDto> UpdateTaskAsync(UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> TaskExistsAsync(int id);
    }
}
