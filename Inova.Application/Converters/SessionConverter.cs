using Inova.Application.DTOs.Session;
using Inova.Application.DTOs.Specialization;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class SessionConverter
{

    public static Session ToEntity(this BookSessionRequestDto dto)
    {
        return new Session
        {
            ConsultantId = dto.ConsultantId,
            ScheduledDate = dto.ScheduledDate,
            ScheduledTime = dto.ScheduledTime,
            DurationHours = dto.DurationHours

        };
    }

    // Direction 2: Entity → ResponseDTO (for READ)

    public static SessionResponseDto ToResponseDto(this Session entity)
    {
        return new SessionResponseDto
        {
            Id = entity.Id,
            ConsultantId = entity.ConsultantId,
            ScheduledDate = entity.ScheduledDate,
            ScheduledTime = entity.ScheduledTime,
            DurationHours = entity.DurationHours,
            TotalAmount = entity.TotalAmount,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }

}
