using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AZR_RED_THREAD_BLL.DTOs.TaskDto
{
    /// <summary>
    /// DTO utilisé pour créer une tâche.
    /// Implémente IValidatableObject pour validation cross-field.
    /// </summary>
    public class CreateTaskDto : IValidatableObject
    {
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
        public int ProjectId { get; set; }

        // Qui crée la tâche (rempli côté API à partir du token)
        public int CreatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
                yield return new ValidationResult("La date de fin doit être postérieure à la date de début", new[] { nameof(EndDate) });

            if (StartDate.Date < DateTime.Today)
                yield return new ValidationResult("La date de début ne peut pas être dans le passé", new[] { nameof(StartDate) });
        }
    }
}
