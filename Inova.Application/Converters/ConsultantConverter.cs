using Inova.Application.DTOs.Consultant;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class ConsultantConverter
{
    public static ConsultantDto ToDto(this Domain.Entities.Consultant consultant)
    {
        return new ConsultantDto
        {
            Id = consultant.Id,
            FullName = consultant.FullName,
            Email = consultant.User?.Email ?? string.Empty,
            PhoneNumber = consultant.PhoneNumber ?? string.Empty,
            SpecializationName = consultant.Specialization?.NameEn ?? string.Empty,
            Bio = consultant.Bio ?? string.Empty,
            YearsOfExperience = consultant.YearsOfExperience,
            HourlyRate = consultant.HourlyRate,
            ApprovalStatus = consultant.ApprovalStatus,
            IsApproved = consultant.IsApproved,
            CreatedAt = consultant.CreatedAt,
            ApprovedAt = consultant.ApprovedAt
        };
    }

    // Add this to your existing ConsultantConverter class

    public static ConsultantPublicProfileDto ToPublicProfileDto(this Consultant consultant)
    {
        return new ConsultantPublicProfileDto
        {
            Id = consultant.Id,
            FullName = consultant.FullName,
            SpecializationName = consultant.Specialization?.NameEn ?? string.Empty,
            Bio = consultant.Bio ?? string.Empty,
            YearsOfExperience = consultant.YearsOfExperience,
            HourlyRate = consultant.HourlyRate,
            ProfileImageUrl = consultant.ProfileImageUrl ?? string.Empty,
            CoverImageUrl = consultant.CoverImageUrl ?? string.Empty,
            TotalSessions = consultant.Sessions?.Count(s => s.Status == "Completed") ?? 0,
            Rating = 0.0  // Future feature
        };
    }
}