using Inova.Application.DTOs.Category;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class CategoryConverter
{
    
    // Direction 1: DTO → Entity (for CREATE)
  
    public static Category ToEntity(this CategoryCreateRequestDto dto)
    {
        return new Category
        {
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl,
            IconUrl = dto.IconUrl,
            CreatedAt = DateTime.UtcNow  // ← Auto-set timestamp
            // ⚠️ Id is NOT set here - database will generate it
        };
    }

    // Direction 2: Entity → ResponseDTO (for READ)
    
    public static CategoryResponseDto ToResponseDto(this Category entity)
    {
        return new CategoryResponseDto
        {
            Id = entity.Id,
            NameAr = entity.NameAr,
            NameEn = entity.NameEn,
            Description = entity.Description,
            CoverImageUrl = entity.CoverImageUrl,
            IconUrl = entity.IconUrl,
            // ↓ Calculate count from navigation property
            SpecializationsCount = entity.Specializations?.Count() ?? 0
            // Breakdown:
            // entity.Specializations  → The list (might be null)
            // ?.Count()              → Count items (? = null-safe)
            // ?? 0                   → If null, return 0
        };
    }

    // Direction 3: UpdateDTO → Entity (for UPDATE)
    public static void UpdateEntity(this CategoryUpdateRequestDto dto, Category entity)
    {
        // Update only the fields that are in the DTO
        entity.NameAr = dto.NameAr;
        entity.NameEn = dto.NameEn;
        entity.Description = dto.Description;
        entity.CoverImageUrl = dto.CoverImageUrl;
        entity.IconUrl = dto.IconUrl;

        // Note We DON'T update Id or CreatedAt!
    }
}