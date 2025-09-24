// AZR_RED_THREAD_BLL/DTOs/UserDto/UserDto.cs
namespace AZR_RED_THREAD_BLL.DTOs.UserDto
{
    /// <summary>
    /// DTO pour exposer un utilisateur côté client.
    /// Contient l'id du rôle et le label du rôle pour affichage.
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? M365UUID { get; set; }

        // Id du rôle (utile pour assignation côté UI)
        public int RoleId { get; set; }

        // Label du rôle (ex: "Admin", "Owner", "User")
        public string? RoleLabel { get; set; }

        // Indicateur d'activité
        public bool IsActive { get; set; }
    }
}
