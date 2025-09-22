using System;

namespace AZR_RED_THREAD_BLL.DTOs.TaskDto
{
    /// <summary>
    /// DTO utilisé pour renvoyer les informations d'une tâche au client.
    /// </summary>
    public class TaskDto
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Statut { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; } // mapping facultatif venant de la relation Project

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }

        // Propriétés calculées utiles côté client
        public int DurationInDays => (EndDate - StartDate).Days;
        public bool IsOverdue => EndDate < DateTime.Now;
    }
}
