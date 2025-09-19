using AZR_RED_THREAD_DAL.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.Project
{

    public class Project : Traceability
    {
        public string Name { get; set; } // Nom du projet
        public string? Description { get; set; } // Description optionnelle du projet
        public DateTime StartDate { get; set; } // Date de début du projet
        public DateTime EndDate { get; set; } // Date de fin du projet
    }
}