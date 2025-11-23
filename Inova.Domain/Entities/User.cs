namespace Inova.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }  // "Customer", "Consultant", "Admin"
    public string ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }  // ← ADD THIS LINE IF MISSING!
}