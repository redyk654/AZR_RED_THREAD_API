using AZR_RED_THREAD_DAL.Services.UserDAServices;
using AZR_RED_THREAD_DAL.Services.RoleDAServices;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.UserServices
{
    public class UserProvisioningService : IUserProvisioningService
    {
        private readonly IUserDAServices _userDA;
        private readonly IRoleDAServices _roleDA;

        // Role label default (vous pouvez le configurer)
        private const string DefaultRoleLabel = "User";
        // CreatedBy fallback (system)
        private const int SystemCreatedById = 1;

        public UserProvisioningService(IUserDAServices userDA, IRoleDAServices roleDA)
        {
            _userDA = userDA;
            _roleDA = roleDA;
        }

        public async Task EnsureUserExistsFromClaimsAsync(ClaimsPrincipal principal)
        {
            if (principal == null) return;

            // Extract oid (Azure AD object id). Try common claim names.
            var oid = principal.FindFirst("oid")?.Value
                      ?? principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                      ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(oid))
            {
                // Can't identify the user — nothing to provision
                return;
            }

            var email = principal.FindFirst("preferred_username")?.Value
                        ?? principal.FindFirst(ClaimTypes.Email)?.Value;

            var displayName = principal.FindFirst("name")?.Value ?? principal.Identity?.Name ?? string.Empty;

            // Try to find user by oid
            var existing = await _userDA.GetByM365UUIDAsync(oid);
            if (existing != null)
            {
                // Optional: update fields if changed
                var changed = false;
                if (!string.IsNullOrWhiteSpace(email) && existing.Email != email)
                {
                    existing.Email = email;
                    changed = true;
                }

                // Update names if absent
                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    var (first, last) = SplitName(displayName);
                    if (!string.IsNullOrWhiteSpace(first) && existing.FirstName != first)
                    {
                        existing.FirstName = first;
                        changed = true;
                    }
                    if (!string.IsNullOrWhiteSpace(last) && existing.LastName != last)
                    {
                        existing.LastName = last;
                        changed = true;
                    }
                }

                if (changed)
                {
                    
                    await _userDA.UpdateUserAsync(existing);
                }
                return;
            }

            // Else create a new user
            var (firstName, lastName) = SplitName(displayName);

            // find role, or create default role
            var role = await _roleDA.GetRoleByLabelAsync(DefaultRoleLabel);
            if (role == null)
            {
                role = new Roles
                {
                    Label = DefaultRoleLabel,
                    Description = "Rôle par défaut",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = SystemCreatedById,
                    IsActive = true
                };
                role = await _roleDA.CreateRoleAsync(role);
            }

            var newUser = new User
            {
                FirstName = firstName,
                LastName = string.IsNullOrWhiteSpace(lastName) ? (lastName ?? "Unknown") : lastName,
                Email = email,
                M365UUID = oid,
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = SystemCreatedById,
                IsActive = true
            };

            await _userDA.CreateUserAsync(newUser);
        }

        private (string? first, string? last) SplitName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName)) return (null, null);
            var parts = displayName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return (parts[0], string.Empty);
            var first = parts[0];
            var last = string.Join(' ', parts.Skip(1));
            return (first, last);
        }
    }
}
