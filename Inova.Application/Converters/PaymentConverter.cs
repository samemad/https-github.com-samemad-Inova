using Inova.Application.DTOs.Category;
using Inova.Domain.Entities;

namespace Inova.Application.Converters;

internal static class PaymentConverter
{
    // Direction 2: Entity → ResponseDTO 

    public static PaymentResponseDto ToResponseDto(this Payment entity)
    {
        return new PaymentResponseDto
        {
            Id = entity.Id,
            SessionId = entity.SessionId,
            Amount = entity.Amount,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            CapturedAt = entity.CapturedAt,   
            ReleasedAt = entity.ReleasedAt

        };
    }

}