using Inova.Application.DTOs.Consultant;
using Inova.Application.Interfaces;
using Inova.Application.Converters;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

internal sealed class ConsultantService : IConsultantService
{
    private readonly IConsultantRepository _consultantRepository;
    private readonly IEmailService _emailService;

    public ConsultantService(
        IConsultantRepository consultantRepository,
        IEmailService emailService)
    {
        _consultantRepository = consultantRepository;
        _emailService = emailService;
    }

    // GET PENDING CONSULTANTS (Admin Only)
    public async Task<IEnumerable<ConsultantDto>> GetPendingConsultantsAsync()
    {
        var pendingConsultants = await _consultantRepository.GetPendingApprovalAsync();
        return pendingConsultants.Select(c => c.ToDto());
    }

    
    // GET CONSULTANT BY ID
    public async Task<ConsultantDto> GetConsultantByIdAsync(int id)
    {
        var consultant = await _consultantRepository.GetByIdAsync(id);

        if (consultant == null)
        {
            throw new InvalidOperationException($"Consultant with ID {id} not found");
        }

        return consultant.ToDto();
    }

    // APPROVE CONSULTANT
    public async Task<bool> ApproveConsultantAsync(int id)
    {
        // 1. Get consultant
        var consultant = await _consultantRepository.GetByIdAsync(id);

        if (consultant == null)
        {
            throw new InvalidOperationException($"Consultant with ID {id} not found");
        }

        // 2. Check if already approved
        if (consultant.IsApproved)
        {
            throw new InvalidOperationException("Consultant is already approved");
        }

        // 3. Update approval status
        consultant.IsApproved = true;
        consultant.ApprovalStatus = "Approved";
        consultant.ApprovedAt = DateTime.UtcNow;

        // 4. Save changes
        await _consultantRepository.UpdateAsync(consultant);

        // 5. Send approval email
        await _emailService.SendConsultantApprovalEmailAsync(
            consultant.User.Email,
            consultant.FullName,
            isApproved: true
        );

        return true;
    }

    // REJECT CONSULTANT
    public async Task<bool> RejectConsultantAsync(int id)
    {
        // 1. Get consultant
        var consultant = await _consultantRepository.GetByIdAsync(id);

        if (consultant == null)
        {
            throw new InvalidOperationException($"Consultant with ID {id} not found");
        }

        // 2. Update rejection status
        consultant.IsApproved = false;
        consultant.ApprovalStatus = "Rejected";
        consultant.ApprovedAt = null;  // Clear approval date if exists

        // 3. Save changes
        await _consultantRepository.UpdateAsync(consultant);

        // 4. Send rejection email
        await _emailService.SendConsultantApprovalEmailAsync(
            consultant.User.Email,
            consultant.FullName,
            isApproved: false
        );

        return true;
    }


    // GET CONSULTANT PUBLIC PROFILE (for customers)
    public async Task<ConsultantPublicProfileDto> GetConsultantPublicProfileAsync(int id)
    {
        var consultant = await _consultantRepository.GetByIdAsync(id);

        if (consultant == null)
        {
            throw new InvalidOperationException($"Consultant with ID {id} not found");
        }

        if (!consultant.IsApproved)
        {
            throw new InvalidOperationException("This consultant is not approved yet");
        }

        return consultant.ToPublicProfileDto();
    }

    // GET ALL APPROVED CONSULTANTS
    public async Task<IEnumerable<ConsultantPublicProfileDto>> GetAllApprovedConsultantsAsync()
    {
        var consultants = await _consultantRepository.GetAllApprovedAsync();
        return consultants.Select(c => c.ToPublicProfileDto());
    }

    // GET CONSULTANTS BY SPECIALIZATION
    public async Task<IEnumerable<ConsultantPublicProfileDto>> GetConsultantsBySpecializationAsync(int specializationId)
    {
        var consultants = await _consultantRepository.GetBySpecializationIdAsync(specializationId);
        return consultants.Select(c => c.ToPublicProfileDto());
    }
}