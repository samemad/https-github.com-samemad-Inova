using Inova.Application.DTOs.Consultant;

namespace Inova.Application.Services;

public interface IConsultantService
{
	Task<IEnumerable<ConsultantDto>> GetPendingConsultantsAsync();
	Task<ConsultantDto> GetConsultantByIdAsync(int id);
	Task<bool> ApproveConsultantAsync(int id);
	Task<bool> RejectConsultantAsync(int id);
}