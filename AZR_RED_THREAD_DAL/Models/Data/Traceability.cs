using System;

namespace AZR_RED_THREAD_DAL.Models.Data
{
    /// <summary>
    /// Classe abstraite pour la traçabilité des entités métiers (non mappée).
    /// </summary>
    public abstract class Traceability
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
