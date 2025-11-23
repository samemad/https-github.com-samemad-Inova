namespace Inova.Domain.Entities;

public class  Category 
{
    public int Id { get; set; }
    public string NameAr { get; set; } // Arabic Name
    public string NameEn { get; set; } // English Name
    public string Description { get; set; } 
    public DateTime CreatedAt { get; set; }
    public string CoverImageUrl { get; set; }  // Cloudinary URL for category cover
    public string IconUrl { get; set; }         // Optional: small icon for category
    public ICollection<Specialization> Specializations { get; set; } // Navigation property to Specializations which means Category can have many Specializations
}

