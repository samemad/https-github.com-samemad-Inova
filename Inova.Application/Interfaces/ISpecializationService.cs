using Inova.Application.DTOs.Specialization;

namespace Inova.Application.Interfaces;

public interface ISpecializationService
{
    Task<SpecializationResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<SpecializationResponseDto>> GetAllAsync();
    Task<IEnumerable<SpecializationResponseDto>> GetByCategoryIdAsync(int categoryId);
    Task<SpecializationResponseDto> CreateAsync(SpecializationCreateDto dto);
    Task<SpecializationResponseDto> UpdateAsync(SpecializationUpdateRequestDto dto);
    Task<bool> DeleteAsync(int id);
}