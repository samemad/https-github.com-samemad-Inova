namespace Inova.Application.DTOs.Category

    public class CategoryUpdateRequestDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string Description { get; set; }

        public string CoverImageUrl { get; set; }  // Cloudinary URL for category cover

        public string IconUrl { get; set; }
    }

