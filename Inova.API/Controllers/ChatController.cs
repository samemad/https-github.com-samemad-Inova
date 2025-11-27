using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inova.Application.DTOs.Chat;
using Inova.Application.Interfaces;
using Inova.Application.DTOs.Auth;
using System.Security.Claims;

namespace Inova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // All chat endpoints require authentication
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/chat/send
    // Send a message (Customer or Consultant)
    // ═══════════════════════════════════════════════════════════
    [HttpPost("send")]
    [Authorize(Roles = "Customer,Consultant")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDto dto)
    {
        try
        {
            // Get userId from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int userId = int.Parse(userIdClaim);

            var message = await _chatService.SendMessageAsync(dto, userId);
            return Ok(message);
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
                "Failed to send message",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/chat/session/{sessionId}
    // Get all messages for a session
    // ═══════════════════════════════════════════════════════════
    [HttpGet("session/{sessionId}")]
    [Authorize(Roles = "Customer,Consultant")]
    public async Task<IActionResult> GetSessionMessages(int sessionId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int userId = int.Parse(userIdClaim);

            var messages = await _chatService.GetSessionMessagesAsync(sessionId, userId);
            return Ok(messages);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ErrorResponseDto(ex.Message, 401));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to retrieve messages",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/chat/{messageId}/mark-read
    // Mark a message as read
    // ═══════════════════════════════════════════════════════════
    [HttpPost("{messageId}/mark-read")]
    [Authorize(Roles = "Customer,Consultant")]
    public async Task<IActionResult> MarkAsRead(int messageId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int userId = int.Parse(userIdClaim);

            var result = await _chatService.MarkAsReadAsync(messageId, userId);
            return Ok(new { message = "Message marked as read", success = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ErrorResponseDto(ex.Message, 401));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponseDto(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to mark message as read",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // GET: api/chat/unread-count
    // Get unread message count for current user
    // ═══════════════════════════════════════════════════════════
    [HttpGet("unread-count")]
    [Authorize(Roles = "Customer,Consultant")]
    public async Task<IActionResult> GetUnreadCount()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int userId = int.Parse(userIdClaim);

            var count = await _chatService.GetUnreadCountAsync(userId);
            return Ok(new { unreadCount = count });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto(
                "Failed to get unread count",
                ex.Message,
                500
            ));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // POST: api/chat/session/{sessionId}/report
    // Report a session (Bonus Feature)
    // ═══════════════════════════════════════════════════════════
    [HttpPost("session/{sessionId}/report")]
    [Authorize(Roles = "Customer,Consultant")]
    public async Task<IActionResult> ReportSession(
        int sessionId,
        [FromBody] ReportSessionRequestDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new ErrorResponseDto("Invalid token", 401));
            }

            int userId = int.Parse(userIdClaim);

            var result = await _chatService.ReportSessionAsync(sessionId, dto, userId);
            return Ok(new
            {
                message = "Session reported successfully. It's now under admin review.",
                success = result
            });
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
                "Failed to report session",
                ex.Message,
                500
            ));
        }
    }
}