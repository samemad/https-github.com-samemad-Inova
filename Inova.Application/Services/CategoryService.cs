using Inova.Application.DTOs.Category;
using Inova.Application.Interfaces;
using Inova.Application.Converters;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

internal sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // GET BY ID
    public async Task<CategoryResponseDto> GetByIdAsync(int id)
    {
        // 1. Get entity from repository
        var category = await _categoryRepository.GetByIdAsync(id);

        // 2. Check if exists
        if (category == null)
        {
            throw new InvalidOperationException($"Category with ID {id} not found");
        }

        // 3. Convert to DTO using converter
        return category.ToResponseDto();
    }

    // GET ALL
    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        // 1. Get all entities from repository
        var categories = await _categoryRepository.GetAllAsync();

        // 2. Convert each entity to DTO
        return categories.Select(c => c.ToResponseDto());

        // Breakdown of Select:
        // categories.Select(c => c.ToResponseDto())
        // ↑ For each category 'c', convert it to DTO
        // Returns a list of DTOs
    }

    // CREATE
    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateRequestDto dto)
    {
        // 1. Validate DTO (basic checks)
        if (string.IsNullOrWhiteSpace(dto.NameAr))
        {
            throw new InvalidOperationException("Arabic name is required");
        }

        if (string.IsNullOrWhiteSpace(dto.NameEn))
        {
            throw new InvalidOperationException("English name is required");
        }

        // 2. Convert DTO to Entity using converter
        var category = dto.ToEntity();

        // 3. Save to database via repository
        await _categoryRepository.AddAsync(category);

        // 4. Convert saved entity back to DTO and return
        return category.ToResponseDto();
    }

    // UPDATE
    public async Task<CategoryResponseDto> UpdateAsync(CategoryUpdateRequestDto dto)
    {
        // 1. Get existing entity from database
        var category = await _categoryRepository.GetByIdAsync(dto.Id);

        // 2. Check if exists
        if (category == null)
        {
            throw new InvalidOperationException($"Category with ID {dto.Id} not found");
        }

        // 3. Update entity properties using converter
        dto.UpdateEntity(category);

        // 4. Save changes via repository
        await _categoryRepository.UpdateAsync(category);

        // 5. Return updated DTO
        return category.ToResponseDto();
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        // 1. Check if exists
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new InvalidOperationException($"Category with ID {id} not found");
        }

        // 2. Delete via repository
        await _categoryRepository.DeleteAsync(id);

        // 3. Return success
        return true;
    }
}