using AZR_RED_THREAD_DAL.Models.Abstracts;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.Task
{
    public class UserTask : Traceability
    {
        public int UserId { get; set; } // Identifiant de l'utilisateur
        public int TaskId { get; set; } // Identifiant de la tâche

        public virtual User? User { get; set; } // Utilisateur associé
        public virtual Task? Task { get; set; } // Tâche associée
    }
}
