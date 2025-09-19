using AZR_RED_THREAD_DAL.Models.Abstracts;
using AZR_RED_THREAD_DAL.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AZR_RED_THREAD_DAL.Models.Task
{
    public class Task : Traceability
    {
        public int Id { get; set; } // Identifier of the task  
        public string Label { get; set; } // Label of the task  
        public string? Description { get; set; } // Description of the task (optional)  
        public DateTime StartDate { get; set; } // Start date  
        public DateTime EndDate { get; set; } // End date  
        public string Statut { get; set; } // Status of the task  
        public int ProjectId { get; set; } // Foreign key to the project  

        public virtual Project.Project Project { get; set; } // Navigation property to the associated project  
    }
}

