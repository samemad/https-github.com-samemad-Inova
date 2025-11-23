namespace Inova.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public int UserId { get; set; }
    public int ProfileId { get; set; }  // CustomerId or ConsultantId
    public DateTime ExpiresAt { get; set; }
    public string Message { get; set; }

    // For Consultant registration - approval status
    public string ApprovalStatus { get; set; }
}