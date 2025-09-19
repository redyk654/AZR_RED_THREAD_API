using AZR_RED_THREAD_DAL.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.State
{
    public class State : Traceability
    {
        public string Label { get; set; } // Libellé de l'état
        public string? Description { get; set; } // Description (optionnel)
    }
}
