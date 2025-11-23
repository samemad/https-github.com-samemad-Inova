using System.Reflection.Metadata;

namespace Inova.Domain.Entities;

public class Specialization
{
    public int Id { get; set; }
    public int CategoryId { get; set; } // Foreign Key to Category entity
    public Category Category { get; set; } // Navigation property to Category entity

    public string NameAr { get; set; } // Arabic Name 
    public string NameEn { get; set; } //  English Name
    public string Description { get; set; }

    // 📸 ADD THIS
    public string IconUrl { get; set; }  // Cloudinary URL for specialization icon

    public DateTime CreatedAt { get; set; }

    public ICollection<Consultant> Consultants { get; set; } // Navigation property to Consultants which means Specialization can have many Consultants
}