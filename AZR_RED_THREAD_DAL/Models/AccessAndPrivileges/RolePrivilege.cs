using AZR_RED_THREAD_DAL.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.AccessAndPrivileges
{
    public class RolePrivilege : Traceability
    {
        public int RoleId { get; set; } // Role ID
        public int PrivilegeId { get; set; } // Privilege ID

        public virtual Roles? Role { get; set; } // Linked role
        public virtual Privilege? Privilege { get; set; } // Linked privilege
    }

}
