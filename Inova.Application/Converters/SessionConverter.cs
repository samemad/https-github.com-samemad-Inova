using Inova.Application.DTOs.Session;
using Inova.Application.DTOs.Specialization;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class SessionConverter
{

    public static Session ToEntity(
      this BookSessionRequestDto dto,
      int customerId,      // ← Service provides this
      decimal totalAmount) // ← Service calculates this
    {
        return new Session
        {
            CustomerId = customerId,  // ← From parameter
            ConsultantId = dto.ConsultantId,
            ScheduledDate = dto.ScheduledDate,
            ScheduledTime = dto.ScheduledTime,
            DurationHours = dto.DurationHours,
            TotalAmount = totalAmount,  // ← From parameter
            Status = "Pending",  // ← Hardcoded
            CreatedAt = DateTime.UtcNow  // ← Auto-set
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
