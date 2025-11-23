using Inova.Application.DTOs.Auth;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class AuthConverter
{
    // Conversion 1: RegisterRequestDto → User Entity
    public static User ToUserEntity(this RegisterRequestDto requestDto, string passwordHash)
    {
        return new User
        {
            Email = requestDto.Email,
            PasswordHash = passwordHash,
            Role = requestDto.Role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Conversion 2: RegisterRequestDto → Customer Entity
    public static Customer ToCustomerEntity(this RegisterRequestDto requestDto, int userId)
    {
        return new Customer
        {
            UserId = userId,  // ← FIXED!
            FullName = requestDto.FullName,
            PhoneNumber = requestDto.PhoneNumber
        };
    }

    // Conversion 3: RegisterRequestDto → Consultant Entity
    public static Consultant ToConsultantEntity(this RegisterRequestDto requestDto, int userId)
    {
        return new Consultant
        {
            UserId = userId,  // ← FIXED!
            FullName = requestDto.FullName,
            PhoneNumber = requestDto.PhoneNumber,
            SpecializationId = requestDto.SpecializationId.Value,
            Bio = requestDto.Bio,
            YearsOfExperience = requestDto.YearsOfExperience.Value,
            HourlyRate = 0,
            ApprovalStatus = "Pending",
            IsApproved = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Conversion 4: Everything → AuthResponseDto
    public static AuthResponseDto ToAuthResponseDto(
        this User user,
        string token,
        string fullName,      // ← Added this parameter!
        int profileId,
        string message,       // ← Added this parameter!
        string? approvalStatus = null)
    {
        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = fullName,  // ← Use the parameter directly!
            Role = user.Role,
            UserId = user.Id,
            ProfileId = profileId,
            ExpiresAt = DateTime.UtcNow.AddHours(24),  // ← Calculate here
            Message = message,
            ApprovalStatus = approvalStatus
        };
    }
}