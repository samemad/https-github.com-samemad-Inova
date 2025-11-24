using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inova.Application.DTOs.Category;
using Inova.Application.Interfaces;
using Inova.Application.DTOs.Auth;

namespace Inova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/category
    // Public - Anyone can view categories
    // ═══════════════════════════════════════════════════════════
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve categories",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/category/{id}
    // Public - Anyone can view a specific category
    // ═══════════════════════════════════════════════════════════
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve category",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/category
    // Admin only - Create new category
    // ═══════════════════════════════════════════════════════════
    [HttpPost]
    [Authorize(Roles = "Admin")]  // ← Only admins can create
    public async Task<IActionResult> Create([FromBody] CategoryCreateRequestDto dto)
    {
        try
        {
            var category = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),           // Route name
                new { id = category.Id },  // Route parameters
                category                   // Response body
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to create category",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // PUT: api/category/{id}
    // Admin only - Update existing category
    // ═══════════════════════════════════════════════════════════
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]  // ← Only admins can update
    public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateRequestDto dto)
    {
        try
        {
            // Ensure ID in URL matches ID in body
            if (id != dto.Id)
            {
                return BadRequest(new ErrorResponseDto(
                    "ID mismatch",
                    "URL ID does not match body ID",
                    400
                ));
            }

            var category = await _categoryService.UpdateAsync(dto);
            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to update category",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // DELETE: api/category/{id}
    // Admin only - Delete category
    // ═══════════════════════════════════════════════════════════
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]  // ← Only admins can delete
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();  // 204 No Content = Success with no body
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to delete category",
                ex.Message,
                500
            ));
        }
    }
}