using Inova.Application.DTOs.Session;
using Inova.Application.Interfaces;
using Inova.Application.Converters;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

internal sealed class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IConsultantRepository _consultantRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentService _paymentService;
    private readonly IEmailService _emailService;

    public SessionService(
        ISessionRepository sessionRepository,
        IConsultantRepository consultantRepository,
        ICustomerRepository customerRepository,
        IPaymentService paymentService,
        IEmailService emailService)
    {
        _sessionRepository = sessionRepository;
        _consultantRepository = consultantRepository;
        _customerRepository = customerRepository;
        _paymentService = paymentService;
        _emailService = emailService;
    }
    // BOOK SESSION (Customer books a session)
    public async Task<SessionResponseDto> BookSessionAsync(BookSessionRequestDto dto, int customerId)
    {
        // 1. Validate consultant exists and is approved
        var consultant = await _consultantRepository.GetByIdAsync(dto.ConsultantId);
        if (consultant == null)
        {
            throw new InvalidOperationException($"Consultant with ID {dto.ConsultantId} not found");
        }

        if (!consultant.IsApproved)
        {
            throw new InvalidOperationException("This consultant is not approved yet");
        }

        // 2. Validate scheduled date is in the future
        if (dto.ScheduledDate.Date < DateTime.UtcNow.Date)
        {
            throw new InvalidOperationException("Cannot book sessions in the past");
        }

        // 3. Validate duration
        if (dto.DurationHours <= 0)
        {
            throw new InvalidOperationException("Duration must be greater than zero");
        }

        // 4. Calculate total amount
        var totalAmount = consultant.HourlyRate * dto.DurationHours;

        // 5. Convert DTO to entity using converter
        var session = dto.ToEntity(customerId, totalAmount);

        // 6. Save session to database
        await _sessionRepository.AddAsync(session);

        // 7. Hold payment
        await _paymentService.HoldPaymentAsync(session.Id, totalAmount);

        // 8. Get customer details for email
        var customer = await _customerRepository.GetByIdAsync(customerId);

        // 9. Send emails
        await _emailService.SendSessionBookedEmailAsync(
            consultant.User.Email,
            customer.FullName,
            session.ScheduledDate,
            session.ScheduledTime
        );

        // 10. Return response DTO
        return session.ToResponseDto();
    }

    
    // ACCEPT SESSION (Consultant accepts)
    public async Task<bool> AcceptSessionAsync(int sessionId, int consultantId)
    {
        // 1. Get session with details
        var session = await _sessionRepository.GetByIdWithDetailsAsync(sessionId);

        if (session == null)
        {
            throw new InvalidOperationException($"Session with ID {sessionId} not found");
        }

        // 2. Security: Verify session belongs to this consultant
        if (session.ConsultantId != consultantId)
        {
            throw new UnauthorizedAccessException("You can only accept your own sessions");
        }

        // 3. Validate status is Pending
        if (session.Status != "Pending")
        {
            throw new InvalidOperationException($"Cannot accept session with status '{session.Status}'");
        }

        // 4. Update session status
        session.Status = "Accepted";
        session.AcceptedAt = DateTime.UtcNow;
        await _sessionRepository.UpdateAsync(session);

        // 5. Capture payment
        var payment = await _paymentService.GetPaymentBySessionIdAsync(sessionId);
        await _paymentService.CapturePaymentAsync(payment.Id);

        // 6. Send email to customer
        await _emailService.SendSessionAcceptedEmailAsync(
            session.Customer.User.Email,
            session.Consultant.FullName,
            session.ScheduledDate,
            session.ScheduledTime
        );

        return true;
    }

    // DENY SESSION (Consultant denies)
    public async Task<bool> DenySessionAsync(int sessionId, int consultantId)
    {
        // 1. Get session with details
        var session = await _sessionRepository.GetByIdWithDetailsAsync(sessionId);

        if (session == null)
        {
            throw new InvalidOperationException($"Session with ID {sessionId} not found");
        }

        // 2. Security: Verify session belongs to this consultant
        if (session.ConsultantId != consultantId)
        {
            throw new UnauthorizedAccessException("You can only deny your own sessions");
        }

        // 3. Validate status is Pending
        if (session.Status != "Pending")
        {
            throw new InvalidOperationException($"Cannot deny session with status '{session.Status}'");
        }

        // 4. Update session status
        session.Status = "Denied";
        await _sessionRepository.UpdateAsync(session);

        // 5. Release payment
        var payment = await _paymentService.GetPaymentBySessionIdAsync(sessionId);
        await _paymentService.ReleasePaymentAsync(payment.Id);

        // 6. Send email to customer
        await _emailService.SendSessionDeniedEmailAsync(
            session.Customer.User.Email,
            session.Consultant.FullName
        );

        return true;
    }

    
    // GET MY SESSIONS (Customer views their sessions)
    public async Task<IEnumerable<SessionResponseDto>> GetMySessionsAsync(int customerId)
    {
        var sessions = await _sessionRepository.GetByCustomerIdAsync(customerId);
        return sessions.Select(s => s.ToResponseDto());
    }

    // GET MY CONSULTANT SESSIONS (Consultant views all their sessions)
    public async Task<IEnumerable<SessionResponseDto>> GetMyConsultantSessionsAsync(int consultantId)
    {
        var sessions = await _sessionRepository.GetByConsultantIdAsync(consultantId);
        return sessions.Select(s => s.ToResponseDto());
    }

    // GET PENDING SESSIONS (Consultant views pending sessions only)
    public async Task<IEnumerable<SessionResponseDto>> GetPendingSessionsAsync(int consultantId)
    {
        var sessions = await _sessionRepository.GetPendingByConsultantIdAsync(consultantId);
        return sessions.Select(s => s.ToResponseDto());
    }

    // GET SESSION BY ID
    public async Task<SessionResponseDto> GetSessionByIdAsync(int id)
    {
        var session = await _sessionRepository.GetByIdAsync(id);

        if (session == null)
        {
            throw new InvalidOperationException($"Session with ID {id} not found");
        }

        return session.ToResponseDto();
    }

    
    // CANCEL SESSION (Customer cancels)
    public async Task<bool> CancelSessionAsync(int sessionId, int customerId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);

        if (session == null)
        {
            throw new InvalidOperationException($"Session with ID {sessionId} not found");
        }

        // Security: Verify session belongs to this customer
        if (session.CustomerId != customerId)
        {
            throw new UnauthorizedAccessException("You can only cancel your own sessions");
        }

        // Can only cancel pending sessions
        if (session.Status != "Pending")
        {
            throw new InvalidOperationException($"Cannot cancel session with status '{session.Status}'");
        }

        session.Status = "Cancelled";
        await _sessionRepository.UpdateAsync(session);

        // Release payment
        var payment = await _paymentService.GetPaymentBySessionIdAsync(sessionId);
        await _paymentService.ReleasePaymentAsync(payment.Id);

        return true;
    }
}