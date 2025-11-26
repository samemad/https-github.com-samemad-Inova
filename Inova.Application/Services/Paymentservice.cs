using Inova.Application.DTOs.Category;
using Inova.Application.Interfaces;
using Inova.Application.Converters;
using Inova.Domain.Entities;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

internal sealed class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    // ═══════════════════════════════════════════════════════════
    // HOLD PAYMENT (Create and hold the amount)
    // ═══════════════════════════════════════════════════════════
    public async Task<PaymentResponseDto> HoldPaymentAsync(int sessionId, decimal amount)
    {
        // 1. Validate amount
        if (amount <= 0)
        {
            throw new InvalidOperationException("Payment amount must be greater than zero");
        }

        // 2. Check if payment already exists for this session
        var existingPayment = await _paymentRepository.GetBySessionIdAsync(sessionId);
        if (existingPayment != null)
        {
            throw new InvalidOperationException($"Payment already exists for session {sessionId}");
        }

        // 3. Create new payment entity
        var payment = new Payment
        {
            SessionId = sessionId,
            Amount = amount,
            Status = "Held",  // ← Initial status
            CreatedAt = DateTime.UtcNow,
            CapturedAt = null,
            ReleasedAt = null
        };

        // 4. Save to database
        await _paymentRepository.AddAsync(payment);

        // 5. Convert to DTO and return
        return payment.ToResponseDto();
    }

    // CAPTURE PAYMENT (Take the money - session accepted)
    public async Task<PaymentResponseDto> CapturePaymentAsync(int paymentId)
    {
        // 1. Get payment from database
        var payment = await _paymentRepository.GetByIdAsync(paymentId);

        // 2. Validate payment exists
        if (payment == null)
        {
            throw new InvalidOperationException($"Payment with ID {paymentId} not found");
        }

        // 3. Validate current status is "Held"
        if (payment.Status != "Held")
        {
            throw new InvalidOperationException(
                $"Cannot capture payment. Current status is '{payment.Status}', expected 'Held'"
            );
        }

        // 4. Update payment status
        payment.Status = "Captured";
        payment.CapturedAt = DateTime.UtcNow;

        // 5. Save changes
        await _paymentRepository.UpdateAsync(payment);

        // 6. Return updated DTO
        return payment.ToResponseDto();
    }

    // RELEASE PAYMENT (Return the money - session denied)
    public async Task<PaymentResponseDto> ReleasePaymentAsync(int paymentId)
    {
        // 1. Get payment from database
        var payment = await _paymentRepository.GetByIdAsync(paymentId);

        // 2. Validate payment exists
        if (payment == null)
        {
            throw new InvalidOperationException($"Payment with ID {paymentId} not found");
        }

        // 3. Validate current status is "Held"
        if (payment.Status != "Held")
        {
            throw new InvalidOperationException(
                $"Cannot release payment. Current status is '{payment.Status}', expected 'Held'"
            );
        }

        // 4. Update payment status
        payment.Status = "Released";
        payment.ReleasedAt = DateTime.UtcNow;

        // 5. Save changes
        await _paymentRepository.UpdateAsync(payment);

        // 6. Return updated DTO
        return payment.ToResponseDto();
    }

    // GET PAYMENT BY SESSION ID
    public async Task<PaymentResponseDto> GetPaymentBySessionIdAsync(int sessionId)
    {
        // 1. Get payment from database
        var payment = await _paymentRepository.GetBySessionIdAsync(sessionId);

        // 2. Validate payment exists
        if (payment == null)
        {
            throw new InvalidOperationException($"No payment found for session {sessionId}");
        }

        // 3. Convert and return
        return payment.ToResponseDto();
    }
}