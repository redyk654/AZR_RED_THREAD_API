using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AZR_RED_THREAD_BLL.DTOs.TaskDto
{
    /// <summary>
    /// DTO utilisé pour mettre à jour une tâche.
    /// </summary>
    public class UpdateTaskDto : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Label { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Statut { get; set; }

        [Required]
        public int ProjectId { get; set; }

        // Qui modifie la tâche (rempli côté API à partir du token)
        public int UpdatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
                yield return new ValidationResult("La date de fin doit être postérieure à la date de début", new[] { nameof(EndDate) });
        }
    }
}
