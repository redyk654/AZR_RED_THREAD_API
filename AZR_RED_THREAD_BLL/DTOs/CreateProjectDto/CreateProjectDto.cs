using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AZR_RED_THREAD_BLL.DTOs.CreateProjectDto
{
    public class CreateProjectDto : IValidatableObject
    {
        [Required(ErrorMessage = "Le nom du projet est obligatoire")]
        [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La date de fin est obligatoire")]
        public DateTime EndDate { get; set; }

        public int CreatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate >= EndDate)
            {
                yield return new ValidationResult(
                    "La date de fin doit être postérieure à la date de début",
                    new[] { nameof(EndDate) });
            }

            if (StartDate < DateTime.Today)
            {
                yield return new ValidationResult(
                    "La date de début ne peut pas être dans le passé",
                    new[] { nameof(StartDate) });
            }
        }
    }
}
