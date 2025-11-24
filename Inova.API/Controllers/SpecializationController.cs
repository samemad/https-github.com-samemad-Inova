using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inova.Application.DTOs.Specialization;
using Inova.Application.Interfaces;
using Inova.Application.DTOs.Auth;

namespace Inova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecializationController : ControllerBase
{
    private readonly ISpecializationService _specializationService;

    public SpecializationController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/specialization
    // Public - Anyone can view specializations
    // ═══════════════════════════════════════════════════════════
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var specializations = await _specializationService.GetAllAsync();
            return Ok(specializations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve specializations",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/specialization/{id}
    // Public - Anyone can view a specific specialization
    // ═══════════════════════════════════════════════════════════
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var specialization = await _specializationService.GetByIdAsync(id);
            return Ok(specialization);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve specialization",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/specialization/category/{categoryId}
    // Public - Filter specializations by category
    // ═══════════════════════════════════════════════════════════
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        try
        {
            var specializations = await _specializationService.GetByCategoryIdAsync(categoryId);
            return Ok(specializations);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve specializations",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/specialization
    // Admin only - Create new specialization
    // ═══════════════════════════════════════════════════════════
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] SpecializationCreateDto dto)
    {
        try
        {
            var specialization = await _specializationService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = specialization.Id },
                specialization
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to create specialization",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // PUT: api/specialization/{id}
    // Admin only - Update existing specialization
    // ═══════════════════════════════════════════════════════════
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] SpecializationUpdateRequestDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest(new ErrorResponseDto(
                    "ID mismatch",
                    "URL ID does not match body ID",
                    400
                ));
            }

            var specialization = await _specializationService.UpdateAsync(dto);
            return Ok(specialization);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to update specialization",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // DELETE: api/specialization/{id}
    // Admin only - Delete specialization
    // ═══════════════════════════════════════════════════════════
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _specializationService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to delete specialization",
                ex.Message,
                500
            ));
        }
    }
}