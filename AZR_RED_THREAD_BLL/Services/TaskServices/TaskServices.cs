using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.TaskDto;
using AZR_RED_THREAD_DAL.Services.TaskDAServices;
using AZR_RED_THREAD_DAL.Services.ProjectDAServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskEntity = AZR_RED_THREAD_DAL.Models.Task.Task;

namespace AZR_RED_THREAD_BLL.Services.TaskServices
{
    /// <summary>
    /// Service métier pour les tâches : orchestration, validations métier, mapping DTO <-> entité.
    /// </summary>
    public class TaskServices : ITaskServices
    {
        private readonly ITaskDAServices _taskDA;
        private readonly IProjectDAServices _projectDA;
        private readonly IMapper _mapper;

        public TaskServices(ITaskDAServices taskDA, IProjectDAServices projectDA, IMapper mapper)
        {
            _taskDA = taskDA;
            _projectDA = projectDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var entities = await _taskDA.GetAllTasksAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(entities);
        }

        public async Task<(IEnumerable<TaskDto> Data, int Total)> GetPaginatedTasksAsync(int page, int pageSize)
        {
            var (entities, total) = await _taskDA.GetPaginatedTasksAsync(page, pageSize);
            var dtos = _mapper.Map<IEnumerable<TaskDto>>(entities);
            return (dtos, total);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var entity = await _taskDA.GetTaskByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<TaskDto>(entity);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
        {
            // Règle métier: le projet doit exister et être actif
            if (!await _projectDA.ProjectExistsAsync(dto.ProjectId))
                throw new InvalidOperationException("Le projet associé n'existe pas.");

            // Règle : label unique pour un même projet
            if (await _taskDA.TaskLabelExistsAsync(dto.Label, dto.ProjectId))
                throw new InvalidOperationException("Une tâche avec le même libellé existe déjà pour ce projet.");

            var entity = _mapper.Map<TaskEntity>(dto);
            entity.CreatedAt = DateTime.Now;
            entity.IsActive = true;

            entity.Statut = "À faire"; // statut par défaut

            var created = await _taskDA.CreateTaskAsync(entity);
            return _mapper.Map<TaskDto>(created);
        }

        public async Task<TaskDto> UpdateTaskAsync(UpdateTaskDto dto)
        {
            // Vérifier que la tâche existe
            var existing = await _taskDA.GetTaskByIdAsync(dto.Id);
            if (existing == null) throw new InvalidOperationException("Tâche introuvable.");

            // Vérifier unicité label dans le projet (exclure l'id courant)
            if (await _taskDA.TaskLabelExistsAsync(dto.Label, dto.ProjectId, dto.Id))
                throw new InvalidOperationException("Une autre tâche avec le même libellé existe dans ce projet.");

            // Map DTO -> entité (gardons l'entité existante pour préserver CreatedAt/CreatedBy)
            existing.Label = dto.Label;
            existing.Description = dto.Description;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.Statut = dto.Statut;
            existing.ProjectId = dto.ProjectId;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = dto.UpdatedBy;

            var updated = await _taskDA.UpdateTaskAsync(existing);
            return _mapper.Map<TaskDto>(updated);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            // Option : vérifier qu'elle existe
            if (!await _taskDA.TaskExistsAsync(id)) return false;
            return await _taskDA.DeleteTaskAsync(id);
        }

        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _taskDA.TaskExistsAsync(id);
        }
    }
}
