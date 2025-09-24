using System.Threading.Tasks;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;

namespace AZR_RED_THREAD_DAL.Services.UserDAServices
{
    /// <summary>
    /// Contrat d'accès aux données pour les utilisateurs (Users table).
    /// Fournit méthodes de lecture nécessaires pour l'auth et l'autorisation.
    /// </summary>
    public interface IUserDAServices
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByM365UUIDAsync(string m365uuid);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
