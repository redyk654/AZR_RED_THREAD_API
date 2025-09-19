using AZR_RED_THREAD_DAL.Models.Abstracts;
using AZR_RED_THREAD_DAL.Models.Project;
using AZR_RED_THREAD_DAL.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.Document
{
    public class Document:Traceability
    {
        public string Label { get; set; } // Nom du fichier
        public string? PathFile { get; set; } // Chemin du fichier (optionnel)
        public int ProjectId { get; set; } // Projet concerné
        public int? TaskId { get; set; } // Tâche concernée (optionnel)

        public virtual Project.Project Project { get; set; } // Projet associé
        public virtual Task.Task? Task { get; set; } // Tâche associée
    }
}
