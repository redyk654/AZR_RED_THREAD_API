namespace AZR_RED_THREAD_BLL.DTOs.UserDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? M365UUID { get; set; }
        public string? RoleLabel { get; set; }
    }
}
