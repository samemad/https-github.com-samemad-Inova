using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inova.Application.Services;
using Inova.Application.DTOs.Auth;

namespace Inova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultantController : ControllerBase
{
    private readonly IConsultantService _consultantService;

    public ConsultantController(IConsultantService consultantService)
    {
        _consultantService = consultantService;
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/consultant/pending
    // Admin only - Get all pending consultant applications
    // ═══════════════════════════════════════════════════════════
    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPending()
    {
        try
        {
            var consultants = await _consultantService.GetPendingConsultantsAsync();
            return Ok(consultants);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve pending consultants",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/consultant/{id}
    // Public - View consultant profile
    // ═══════════════════════════════════════════════════════════
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var consultant = await _consultantService.GetConsultantByIdAsync(id);
            return Ok(consultant);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve consultant",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/consultant/{id}/approve
    // Admin only - Approve consultant application
    // ═══════════════════════════════════════════════════════════
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(int id)
    {
        try
        {
            var result = await _consultantService.ApproveConsultantAsync(id);
            return Ok(new
            {
                message = "Consultant approved successfully",
                success = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to approve consultant",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/consultant/{id}/reject
    // Admin only - Reject consultant application
    // ═══════════════════════════════════════════════════════════
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reject(int id)
    {
        try
        {
            var result = await _consultantService.RejectConsultantAsync(id);
            return Ok(new
            {
                message = "Consultant rejected successfully",
                success = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to reject consultant",
                ex.Message,
                500
            ));
        }
    }

    // Add to existing ConsultantController

    // ═══════════════════════════════════════════════════════════
    // GET: api/consultant/profile/{id}
    // Public - Customer views consultant profile
    // ═══════════════════════════════════════════════════════════
    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetConsultantProfile(int id)
    {
        try
        {
            var consultant = await _consultantService.GetConsultantPublicProfileAsync(id);
            return Ok(consultant);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve consultant profile",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/consultant/approved
    // Public - List all approved consultants
    // ═══════════════════════════════════════════════════════════
    [HttpGet("approved")]
    public async Task<IActionResult> GetApprovedConsultants()
    {
        try
        {
            var consultants = await _consultantService.GetAllApprovedConsultantsAsync();
            return Ok(consultants);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve consultants",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/consultant/specialization/{specializationId}
    // Public - Filter consultants by specialization
    // ═══════════════════════════════════════════════════════════
    [HttpGet("specialization/{specializationId}")]
    public async Task<IActionResult> GetConsultantsBySpecialization(int specializationId)
    {
        try
        {
            var consultants = await _consultantService.GetConsultantsBySpecializationAsync(specializationId);
            return Ok(consultants);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve consultants",
                ex.Message,
                500
            ));
        }
    }
}