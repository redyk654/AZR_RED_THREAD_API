using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.TaskDto;
using AZR_RED_THREAD_BLL.Services.TaskServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AZR_RED_THREAD_API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskServices _taskServices;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskServices taskServices, ILogger<TasksController> logger)
        {
            _taskServices = taskServices;
            _logger = logger;
        }

        // GET api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskServices.GetAllTasksAsync();
            return Ok(tasks);
        }

        // GET api/tasks/paginate?page=1&pageSize=10
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _taskServices.GetPaginatedTasksAsync(page, pageSize);
            return Ok(new { data = result.Data, total = result.Total, page, pageSize });
        }

        // GET api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskServices.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // POST api/tasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Récupérer userId depuis le token si disponible (ici fallback)
                var userId = GetCurrentUserId();
                dto.CreatedBy = userId;

                var created = await _taskServices.CreateTaskAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating task");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating task");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                if (id != dto.Id) return BadRequest("Id mismatch");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // user id from token
                dto.UpdatedBy = GetCurrentUserId();

                var updated = await _taskServices.UpdateTaskAsync(dto);
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating task {TaskId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating task {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _taskServices.DeleteTaskAsync(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while deleting task {TaskId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting task {TaskId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // Récupérer l'ID utilisateur depuis le token (même méthode que pour projects)
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogWarning("User id not found in token, using fallback id=1");
                return 1;
            }
            return userId;
        }
    }
}
