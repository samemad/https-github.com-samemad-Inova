using Inova.Application.Converters;
using Inova.Application.DTOs.Profile;
using Inova.Application.Interfaces;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

public class ProfileService : IProfileService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IConsultantRepository _consultantRepository;
    private readonly IUserRepository _userRepository;

    public ProfileService(
        ICustomerRepository customerRepository,
        IConsultantRepository consultantRepository,
        IUserRepository userRepository)
    {
        _customerRepository = customerRepository;
        _consultantRepository = consultantRepository;
        _userRepository = userRepository;
    }

    // GET my profile
    public async Task<object> GetMyProfileAsync(int userId, string role)
    {
        if (role == "Customer")
        {
            // Get customer by userId (not by customer.Id!)
            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
                throw new Exception("Customer profile not found");

            return customer.ToCustomerProfileDto();
        }
        else if (role == "Consultant")
        {
            // Get consultant by userId (not by consultant.Id!)
            var consultant = await _consultantRepository.GetByUserIdAsync(userId);

            if (consultant == null)
                throw new Exception("Consultant profile not found");

            return consultant.ToConsultantProfileDto();
        }
        else
        {
            throw new Exception("Invalid role");
        }
    }

    // UPDATE my profile
    public async Task<object> UpdateMyProfileAsync(int userId, string role, object updateDto)
    {
        if (role == "Customer")
        {
            // Cast the object to the correct DTO type
            var dto = updateDto as UpdateCustomerProfileDto;
            if (dto == null)
                throw new Exception("Invalid DTO type for Customer");

            // Get customer and user entities
            var customer = await _customerRepository.GetByUserIdAsync(userId);
            if (customer == null)
                throw new Exception("Customer profile not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Update entities using extension method
            dto.UpdateCustomerEntity(customer, user);

            // Save changes
            await _customerRepository.UpdateAsync(customer);
            await _userRepository.UpdateAsync(user);

            // Return updated profile
            return customer.ToCustomerProfileDto();
        }
        else if (role == "Consultant")
        {
            // Cast the object to the correct DTO type
            var dto = updateDto as UpdateConsultantProfileDto;
            if (dto == null)
                throw new Exception("Invalid DTO type for Consultant");

            // Get consultant entity
            var consultant = await _consultantRepository.GetByUserIdAsync(userId);
            if (consultant == null)
                throw new Exception("Consultant profile not found");

            // Update entity using extension method
            dto.UpdateConsultantEntity(consultant);

            // Save changes
            await _consultantRepository.UpdateAsync(consultant);

            // Return updated profile
            return consultant.ToConsultantProfileDto();
        }
        else
        {
            throw new Exception("Invalid role");
        }
    }

    // DELETE my account (soft delete)
    public async Task<bool> DeleteMyAccountAsync(int userId, string role)
    {
        try
        {
            // Get the user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            // Soft delete: Set IsActive to false
            user.IsActive = false;

            // Save changes
            await _userRepository.UpdateAsync(user);

            return true;
        }
        catch
        {
            return false;
        }
    }
}