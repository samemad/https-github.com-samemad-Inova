//using Inova.Application.DTOs.Consultant;
//using Inova.Application.Interfaces;
//using Inova.Application.Converters;
//using Inova.Domain.Repositories;

//namespace Inova.Application.Services;

//internal sealed class ConsultantService : IConsultantService
//{
//    private readonly IConsultantRepository _consultantRepository;
//    private readonly IEmailService _emailService;

//    public ConsultantService(
//        IConsultantRepository consultantRepository,
//        IEmailService emailService)
//    {
//        _consultantRepository = consultantRepository;
//        _emailService = emailService;
//    }

//    public async Task<IEnumerable<ConsultantDto>> GetPendingConsultantsAsync()
//    {
//        // 1. Get all pending consultants from repository
//        var pendingConsultants = await _consultantRepository.GetPendingApprovalAsync();

//        // 2. Convert each to DTO using converter
//        var consultantDtos = pendingConsultants.Select(c => c.ToDto());

//        // 3. Return list
//        return consultantDtos;
//    }

//   // public async Task<ConsultantDto> GetConsultantByIdAsync(int id)
//    {
//        // TODO:
//        // 1. Get consultant by ID from repository
//        // 2. Check if null
//        // 3. Convert to DTO
//        // 4. Return
//    }

//   // public async Task<bool> ApproveConsultantAsync(int id)
//    {
//        // TODO:
//        // 1. Get consultant by ID
//        // 2. Check if null
//        // 3. Check if already approved
//        // 4. Update: IsApproved = true, ApprovalStatus = "Approved", ApprovedAt = DateTime.UtcNow
//        // 5. Save to repository
//        // 6. Send approval email
//        // 7. Return true
//    }

//    //public async Task<bool> RejectConsultantAsync(int id)
//    {
//        // TODO:
//        // 1. Get consultant by ID
//        // 2. Check if null
//        // 3. Update: IsApproved = false, ApprovalStatus = "Rejected"
//        // 4. Save to repository
//        // 5. Send rejection email
//        // 6. Return true
//    }
//}