using AZR_RED_THREAD_BLL.DTOs;
using AZR_RED_THREAD_BLL.Services.ProjectServices;
using AZR_RED_THREAD_BLL.DTOs.CreateProjectDto;
using AZR_RED_THREAD_BLL.DTOs.PaginatedResult;
using AZR_RED_THREAD_BLL.DTOs.ProjectDto;
using AZR_RED_THREAD_BLL.DTOs.UpdateProjectDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AZR_RED_THREAD_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectServices projectServices, ILogger<ProjectController> logger)
        {
            _projectServices = projectServices;
            _logger = logger;
        }

        /// <summary>
        /// Récupère tous les projets
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
        {
            try
            {
                var projects = await _projectServices.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de tous les projets");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les projets avec pagination
        /// </summary>
        [HttpGet("paginate")]
        public async Task<ActionResult<PaginatedResult<ProjectDto>>> GetPaginatedProjects(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            try
            {
                var result = await _projectServices.GetPaginatedProjectsAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération paginée des projets");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère un projet par son ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            try
            {
                var project = await _projectServices.GetProjectByIdAsync(id);

                if (project == null)
                {
                    return NotFound($"Le projet avec l'ID {id} n'a pas été trouvé");
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du projet {ProjectId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Crée un nouveau projet
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Récupérer l'ID utilisateur depuis le token JWT
                var userId = GetCurrentUserId();
                createProjectDto.CreatedBy = userId;

                var createdProject = await _projectServices.CreateProjectAsync(createProjectDto);

                _logger.LogInformation("Projet créé avec succès: {ProjectId} par utilisateur {UserId}",
                    createdProject.Id, userId);

                return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erreur de validation lors de la création du projet");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du projet");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Met à jour un projet existant
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int id, [FromBody] UpdateProjectDto updateProjectDto)
        {
            try
            {
                if (id != updateProjectDto.Id)
                {
                    return BadRequest("L'ID dans l'URL ne correspond pas à l'ID dans le body");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Récupérer l'ID utilisateur depuis le token JWT
                updateProjectDto.UpdatedBy = GetCurrentUserId();

                var updatedProject = await _projectServices.UpdateProjectAsync(updateProjectDto);

                _logger.LogInformation("Projet mis à jour avec succès: {ProjectId} par utilisateur {UserId}",
                    id, GetCurrentUserId());

                return Ok(updatedProject);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erreur de validation lors de la mise à jour du projet {ProjectId}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du projet {ProjectId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Supprime un projet (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            try
            {
                var result = await _projectServices.DeleteProjectAsync(id);

                if (!result)
                {
                    return NotFound($"Le projet avec l'ID {id} n'a pas été trouvé");
                }

                _logger.LogInformation("Projet supprimé avec succès: {ProjectId} par utilisateur {UserId}",
                    id, GetCurrentUserId());

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erreur lors de la suppression du projet {ProjectId}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du projet {ProjectId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère l'ID de l'utilisateur actuel depuis le token JWT
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                // Fallback si l'ID n'est pas disponible (à ajuster selon votre logique)
                _logger.LogWarning("ID utilisateur non trouvé dans le token, utilisation de l'ID par défaut");
                return 1; // Ou lever une exception selon vos besoins
            }

            return userId;
        }
    }
}
