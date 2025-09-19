using AZR_RED_THREAD_DAL.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.AccessAndPrivileges
{
    public class Privilege : Traceability
    {
        public string Label { get; set; } // Privilege label
        public string Description { get; set; } // Privilege description
    }
}
