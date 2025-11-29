using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Inova.Application.DTOs.Profile;
using Inova.Application.Interfaces;

namespace Inova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // All endpoints require authentication
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    // GET: api/profile/me
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            // Get userId and role from JWT token
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);

            // Get profile
            var profile = await _profileService.GetMyProfileAsync(userId, role);

            return Ok(new
            {
                success = true,
                data = profile
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    // PUT: api/profile/me
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] object updateDto)
    {
        try
        {
            // Get userId and role from JWT token
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);

            // Deserialize to correct DTO based on role
            object dto;
            if (role == "Customer")
            {
                dto = System.Text.Json.JsonSerializer.Deserialize<UpdateCustomerProfileDto>(
                    updateDto.ToString(),
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            else if (role == "Consultant")
            {
                dto = System.Text.Json.JsonSerializer.Deserialize<UpdateConsultantProfileDto>(
                    updateDto.ToString(),
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid role" });
            }

            // Update profile
            var updatedProfile = await _profileService.UpdateMyProfileAsync(userId, role, dto);

            return Ok(new
            {
                success = true,
                message = "Profile updated successfully",
                data = updatedProfile
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    // DELETE: api/profile/me
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyAccount()
    {
        try
        {
            // Get userId and role from JWT token
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);

            // Delete account (soft delete)
            var result = await _profileService.DeleteMyAccountAsync(userId, role);

            if (result)
            {
                return Ok(new
                {
                    success = true,
                    message = "Account deleted successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to delete account"
                });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}