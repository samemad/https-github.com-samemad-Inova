using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inova.Application.DTOs.Session;
using Inova.Application.Interfaces;
using Inova.Application.DTOs.Auth;
using System.Security.Claims;

namespace Inova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // All endpoints require authentication
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/session/book
    // Customer books a session
    // ═══════════════════════════════════════════════════════════
    [HttpPost("book")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> BookSession([FromBody] BookSessionRequestDto dto)
    {
        try
        {
            // Get customerId from JWT token
            var customerIdClaim = User.FindFirst("ProfileId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int customerId = int.Parse(customerIdClaim);

            var session = await _sessionService.BookSessionAsync(dto, customerId);
            return Ok(session);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to book session",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/session/my-sessions
    // Customer views their sessions
    // ═══════════════════════════════════════════════════════════
    [HttpGet("my-sessions")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMySessions()
    {
        try
        {
            var customerIdClaim = User.FindFirst("ProfileId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int customerId = int.Parse(customerIdClaim);

            var sessions = await _sessionService.GetMySessionsAsync(customerId);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve sessions",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/session/{id}/cancel
    // Customer cancels their session
    // ═══════════════════════════════════════════════════════════
    [HttpPost("{id}/cancel")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CancelSession(int id)
    {
        try
        {
            var customerIdClaim = User.FindFirst("ProfileId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int customerId = int.Parse(customerIdClaim);

            var result = await _sessionService.CancelSessionAsync(id, customerId);
            return Ok(new { message = "Session cancelled successfully", success = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ErrorResponseDto(ex.Message, 401));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to cancel session",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/session/consultant/sessions
    // Consultant views all their sessions
    // ═══════════════════════════════════════════════════════════
    [HttpGet("consultant/sessions")]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> GetConsultantSessions()
    {
        try
        {
            var consultantIdClaim = User.FindFirst("ProfileId")?.Value;
            if (string.IsNullOrEmpty(consultantIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int consultantId = int.Parse(consultantIdClaim);

            var sessions = await _sessionService.GetMyConsultantSessionsAsync(consultantId);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve sessions",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/session/consultant/pending
    // Consultant views pending sessions only
    // ═══════════════════════════════════════════════════════════
    [HttpGet("consultant/pending")]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> GetPendingSessions()
    {
        try
        {
            var consultantIdClaim = User.FindFirst("ProfileId")?.Value;
            if (string.IsNullOrEmpty(consultantIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int consultantId = int.Parse(consultantIdClaim);

            var sessions = await _sessionService.GetPendingSessionsAsync(consultantId);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve pending sessions",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/session/{id}/accept
    // Consultant accepts a session
    // ═══════════════════════════════════════════════════════════
    [HttpPost("{id}/accept")]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> AcceptSession(int id)
    {
        try
        {
            var consultantIdClaim = User.FindFirst("ProfileId")?.Value;
            if (string.IsNullOrEmpty(consultantIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int consultantId = int.Parse(consultantIdClaim);

            var result = await _sessionService.AcceptSessionAsync(id, consultantId);
            return Ok(new { message = "Session accepted successfully", success = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ErrorResponseDto(ex.Message, 401));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to accept session",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/session/{id}/deny
    // Consultant denies a session
    // ═══════════════════════════════════════════════════════════
    [HttpPost("{id}/deny")]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> DenySession(int id)
    {
        try
        {
            var consultantIdClaim = User.FindFirst("ProfileId")?.Value;
            if (string.IsNullOrEmpty(consultantIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int consultantId = int.Parse(consultantIdClaim);

            var result = await _sessionService.DenySessionAsync(id, consultantId);
            return Ok(new { message = "Session denied successfully", success = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ErrorResponseDto(ex.Message, 401));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to deny session",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/session/{id}
    // Get session details by ID
    // ═══════════════════════════════════════════════════════════
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var session = await _sessionService.GetSessionByIdAsync(id);
            return Ok(session);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve session",
                ex.Message,
                500
            ));
        }
    }
}