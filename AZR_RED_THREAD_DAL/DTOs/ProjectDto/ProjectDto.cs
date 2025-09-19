using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.DTOs.ProjectDto
{
    /// <summary>
    /// DTO utilisé pour renvoyer les projets au client (lecture seule).
    /// </summary>
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Propriétés calculées
        public int DurationInDays => (EndDate - StartDate).Days;
        public bool IsOverdue => EndDate < DateTime.Now;
        public string Status => GetProjectStatus();

        private string GetProjectStatus()
        {
            var now = DateTime.Now;
            if (now < StartDate) return "À venir";
            if (now > EndDate) return "Terminé";
            return "En cours";
        }
}

