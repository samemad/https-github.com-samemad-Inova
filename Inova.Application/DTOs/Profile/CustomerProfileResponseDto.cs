namespace Inova.Application.DTOs.Profile;

public class CustomerProfileResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfileImageUrl { get; set; }
}