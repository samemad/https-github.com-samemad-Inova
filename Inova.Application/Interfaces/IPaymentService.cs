using Inova.Application.DTOs.Category;

namespace Inova.Application.Interfaces;

public interface IPaymentService
{
    // Create a payment and hold the amount
    Task<PaymentResponseDto> HoldPaymentAsync(int sessionId, decimal amount);

    // Capture the payment (take the money)
    Task<PaymentResponseDto> CapturePaymentAsync(int paymentId);

    // Release the payment (return the money)
    Task<PaymentResponseDto> ReleasePaymentAsync(int paymentId);

    // Get payment by session ID
    Task<PaymentResponseDto> GetPaymentBySessionIdAsync(int sessionId);

}