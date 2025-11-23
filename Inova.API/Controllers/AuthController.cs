using Microsoft.AspNetCore.Mvc;
using Inova.Application.DTOs.Auth;
using Inova.Application.Services;

namespace Inova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Register a new user (Customer or Consultant)
    /// POST /api/auth/register
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
    {
        try
        {
            var response = await _authService.RegisterAsync(requestDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto(ex.Message, 400));
        }
        catch (Exception ex)
        {
            // 👇 ADD THIS LINE TO SEE THE FULL ERROR
            Console.WriteLine($"Registration Error: {ex}");

            return StatusCode(500, new ErrorResponseDto(
                "An error occurred during registration",
                ex.InnerException?.Message ?? ex.Message,  // ← Changed this line
                500
            ));
        }
    }
    /// <summary>
    /// Login existing user
    /// POST /api/auth/login
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
    {
        try
        {
            var response = await _authService.LoginAsync(requestDto);
            return Ok(response);
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
                "An error occurred during login",
                ex.Message,
                500
            ));
        }
    }
}