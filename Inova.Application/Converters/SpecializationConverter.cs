using Inova.Application.DTOs.Specialization;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class SpecializationConverter
{
    
    // Direction 1: CreateDTO → Entity (for CREATE)
   
    public static Specialization ToEntity(this SpecializationCreateDto dto)
    {
        return new Specialization
        {
            CategoryId = dto.CategoryId,
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            Description = dto.Description,
            IconUrl = dto.IconUrl,
            CreatedAt = DateTime.UtcNow
        };
    }

    
    // Direction 2: Entity → ResponseDTO (for READ)
  
    public static SpecializationResponseDto ToResponseDto(this Specialization entity)
    {
        return new SpecializationResponseDto
        {
            Id = entity.Id,
            CategoryId = entity.CategoryId,
            NameAr = entity.NameAr,
            NameEn = entity.NameEn,
            Description = entity.Description,
            IconUrl = entity.IconUrl
        };
    }

    
    // Direction 3: UpdateDTO → Entity (for UPDATE)
   
    public static void UpdateEntity(this SpecializationUpdateRequestDto dto, Specialization entity)
    {
        entity.CategoryId = dto.CategoryId;
        entity.NameAr = dto.NameAr;
        entity.NameEn = dto.NameEn;
        entity.Description = dto.Description;
        entity.IconUrl = dto.IconUrl;
    }
}