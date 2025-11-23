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
}