using System.Security.Claims;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_BLL.Services.UserServices
{
    /// <summary>
    /// Service responsable de créer / synchroniser un user DB
    /// à partir des claims présents dans le token Azure AD (principal).
    /// </summary>
    public interface IUserProvisioningService
    {
        /// <summary>
        /// Vérifie si l'utilisateur (principal) existe en base (M365 UUID / oid).
        /// Si non : crée le user (avec rôle par défaut 'User').
        /// Si oui : met à jour email/nom si nécessaire.
        /// </summary>
        Task EnsureUserExistsFromClaimsAsync(ClaimsPrincipal principal);
    }
}
