namespace Inova.Application.DTOs.Profile;

public class UpdateCustomerProfileDto
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfileImageUrl { get; set; } // Optional
}