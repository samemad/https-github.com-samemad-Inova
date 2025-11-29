using Inova.Application.DTOs.Category;
using Inova.Application.DTOs.Profile;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class ProfileConverter
{
    public static CustomerProfileResponseDto ToCustomerProfileDto(
    this Customer customer)
    {
        return new CustomerProfileResponseDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.User?.Email ?? string.Empty,  // ✅ Null-safe
            PhoneNumber = customer.PhoneNumber,
            ProfileImageUrl = customer.User?.ProfileImageUrl ?? string.Empty  // ✅ Null-safe
        };
    }

    public static ConsultantProfileResponseDto ToConsultantProfileDto(
    this Consultant consultant)
    {
        return new ConsultantProfileResponseDto
        {
            Id = consultant.Id,
            FullName = consultant.FullName,
            Email = consultant.User?.Email ?? string.Empty,  // ✅ Null-safe
            PhoneNumber = consultant.PhoneNumber,
            Bio = consultant.Bio,
            YearsOfExperience = consultant.YearsOfExperience,
            HourlyRate = consultant.HourlyRate,
            SpecializationName = consultant.Specialization?.NameEn ?? string.Empty, // ✅ Null-safe
            TotalSessions = consultant.Sessions?.Count ?? 0, // ✅ Null-safe
            ProfileImageUrl = consultant.ProfileImageUrl ?? string.Empty, // Consultant has its own!
            ApprovalStatus = consultant.ApprovalStatus,
            IsApproved = consultant.IsApproved,
            Rating = 0.0 // Placeholder, implement rating calculation if needed
        };
    }

    public static void UpdateCustomerEntity(
     this UpdateCustomerProfileDto dto,
     Customer customer,
     User user)
    {
        // Update Customer entity properties
        customer.FullName = dto.FullName;
        customer.PhoneNumber = dto.PhoneNumber;

        // Update User entity property (only if provided)
        user.ProfileImageUrl = dto.ProfileImageUrl ?? user.ProfileImageUrl;
        //   ↑ If dto.ProfileImageUrl is null, keep the old value
    }
    public static void UpdateConsultantEntity(
      this UpdateConsultantProfileDto dto,
      Consultant consultant)
    {
        // Update basic properties
        consultant.FullName = dto.FullName;
        consultant.PhoneNumber = dto.PhoneNumber;
        consultant.Bio = dto.Bio;
        consultant.YearsOfExperience = dto.YearsOfExperience;
        consultant.HourlyRate = dto.HourlyRate;

        // Update image URLs (only if provided, otherwise keep old values)
        consultant.ProfileImageUrl = dto.ProfileImageUrl ?? consultant.ProfileImageUrl;
        consultant.CoverImageUrl = dto.CoverImageUrl ?? consultant.CoverImageUrl;
        consultant.CertificateImageUrl = dto.CertificateImageUrl ?? consultant.CertificateImageUrl;

        // DO NOT UPDATE:
        // - SpecializationId (requires admin)
        // - ApprovalStatus (admin-only)
        // - IsApproved (admin-only)
    }
}