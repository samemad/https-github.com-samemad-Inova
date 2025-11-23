namespace Inova.Domain.Entities;

    public class Customer

{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign Key to User entity
    public User User { get; set; } // Navigation property to User entity 
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public ICollection<Session> Sessions { get; set; } // Navigation property to Sessions which means Customer can have many Sessions 
}