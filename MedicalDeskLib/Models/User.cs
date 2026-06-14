namespace MedicalDeskLib.Models;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = "";

    public string Phone { get; set; } = "";

    public string Login { get; set; } = "";

    public string PasswordHash { get; set; } = "";

    public int RoleId { get; set; }

    public string RoleName { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastLogin { get; set; }
}