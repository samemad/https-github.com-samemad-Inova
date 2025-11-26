using Inova.Application.DTOs.Specialization;
using Inova.Application.Interfaces;
using Inova.Application.Converters;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

internal sealed class SpecializationService : ISpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;
    private readonly ICategoryRepository _categoryRepository;

    public SpecializationService(
        ISpecializationRepository specializationRepository,
        ICategoryRepository categoryRepository)
    {
        _specializationRepository = specializationRepository;
        _categoryRepository = categoryRepository;
    }

    // GET BY ID
    public async Task<SpecializationResponseDto> GetByIdAsync(int id)
    {
        var specialization = await _specializationRepository.GetByIdAsync(id);

        if (specialization == null)
        {
            throw new InvalidOperationException($"Specialization with ID {id} not found");
        }

        return specialization.ToResponseDto();
    }

    // GET ALL
    public async Task<IEnumerable<SpecializationResponseDto>> GetAllAsync()
    {
        var specializations = await _specializationRepository.GetAllAsync();
        return specializations.Select(s => s.ToResponseDto());
    }

    // GET BY CATEGORY ID (FILTER)
    public async Task<IEnumerable<SpecializationResponseDto>> GetByCategoryIdAsync(int categoryId)
    {
        // First check if category exists
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with ID {categoryId} not found");
        }

        var specializations = await _specializationRepository.GetByCategoryIdAsync(categoryId);
        return specializations.Select(s => s.ToResponseDto());
    }

    // CREATE
    public async Task<SpecializationResponseDto> CreateAsync(SpecializationCreateDto dto)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(dto.NameAr))
        {
            throw new InvalidOperationException("Arabic name is required");
        }

        if (string.IsNullOrWhiteSpace(dto.NameEn))
        {
            throw new InvalidOperationException("English name is required");
        }

        // Check if category exists
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with ID {dto.CategoryId} not found");
        }

        // Convert and save
        var specialization = dto.ToEntity();
        await _specializationRepository.AddAsync(specialization);

        return specialization.ToResponseDto();
    }

    // UPDATE
    public async Task<SpecializationResponseDto> UpdateAsync(SpecializationUpdateRequestDto dto)
    {
        // Get existing
        var specialization = await _specializationRepository.GetByIdAsync(dto.Id);
        if (specialization == null)
        {
            throw new InvalidOperationException($"Specialization with ID {dto.Id} not found");
        }

        // Check if new category exists (if changed)
        if (dto.CategoryId != specialization.CategoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
            {
                throw new InvalidOperationException($"Category with ID {dto.CategoryId} not found");
            }
        }

        // Update and save
        dto.UpdateEntity(specialization);
        await _specializationRepository.UpdateAsync(specialization);

        return specialization.ToResponseDto();
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var specialization = await _specializationRepository.GetByIdAsync(id);
        if (specialization == null)
        {
            throw new InvalidOperationException($"Specialization with ID {id} not found");
        }

        await _specializationRepository.DeleteAsync(id);
        return true;
    }
}