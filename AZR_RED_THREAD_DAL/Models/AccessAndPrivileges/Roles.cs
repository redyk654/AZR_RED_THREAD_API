using AZR_RED_THREAD_DAL.Models.Abstracts;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.AccessAndPrivileges
{
    public class Roles : Traceability

    {
        public string Label { get; set; } // Role label
        public string? Description { get; set; } // Role description (optional)

    }
}
