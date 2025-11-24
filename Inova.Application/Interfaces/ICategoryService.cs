using Inova.Application.DTOs.Category;

namespace Inova.Application.Interfaces;

public interface ICategoryService
{
    // READ operations
    Task<CategoryResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();

    // CREATE operation
    Task<CategoryResponseDto> CreateAsync(CategoryCreateRequestDto dto);

    // UPDATE operation
    Task<CategoryResponseDto> UpdateAsync(CategoryUpdateRequestDto dto);

    // DELETE operation
    Task<bool> DeleteAsync(int id);
}