namespace Inova.Application.Interfaces;

public interface IProfileService
{
    // GET profile - returns object (can be Customer or Consultant DTO)
    Task<object> GetMyProfileAsync(int userId, string role);

    // UPDATE profile - accepts object (can be Customer or Consultant DTO)
    Task<object> UpdateMyProfileAsync(int userId, string role, object updateDto);

    // DELETE account - soft delete
    Task<bool> DeleteMyAccountAsync(int userId, string role);
}