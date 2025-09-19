using AZR_RED_THREAD_DAL.Models.Abstracts;
using AZR_RED_THREAD_DAL.Models.Task;


namespace AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users
{
    public class User : Traceability
    {
        public string? FirstName { get; set; } // First name (optional)
        public string LastName { get; set; } // Last name
        public string? Email { get; set; } // Email address (optional)
        public string M365UUID { get; set; } // Microsoft Entra ID (M365 UUID)
        public int RoleId { get; set; } // Foreign key to Role
        public virtual Roles? Role { get; set; } // Associated role
    }

}
