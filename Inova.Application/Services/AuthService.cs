using BCrypt.Net;
using Inova.Application.DTOs.Auth;
using Inova.Application.Interfaces;
using Inova.Domain.Entities;
using Inova.Domain.Repositories;

namespace Inova.Application.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto requestDto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto requestDto);
}

internal sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IConsultantRepository _consultantRepository;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AuthService(
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        IConsultantRepository consultantRepository,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _consultantRepository = consultantRepository;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto requestDto)
    {
        // 1. Validate request
        if (!requestDto.IsValid())
        {
            throw new InvalidOperationException(requestDto.GetValidationErrors());
        }

        // 2. Check if email already exists
        var emailExists = await _userRepository.EmailExistsAsync(requestDto.Email);
        if (emailExists)
        {
            throw new InvalidOperationException("Email already exists");
        }

        // 3. Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(requestDto.Password);

        // 4. Create User entity
        var user = new User
        {
            Email = requestDto.Email,
            PasswordHash = passwordHash,
            Role = requestDto.Role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        // 5. Create Customer or Consultant profile
        int profileId = 0;
        string approvalStatus = null;

        if (requestDto.Role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
        {
            var customer = new Customer
            {
                UserId = user.Id,
                FullName = requestDto.FullName,
                PhoneNumber = requestDto.PhoneNumber
            };

            await _customerRepository.AddAsync(customer);
            profileId = customer.Id;

            // Send welcome email
            await _emailService.SendWelcomeEmailAsync(user.Email, requestDto.FullName);
        }
        else if (requestDto.Role.Equals("Consultant", StringComparison.OrdinalIgnoreCase))
        {
            var consultant = new Consultant
            {
                UserId = user.Id,
                FullName = requestDto.FullName,
                PhoneNumber = requestDto.PhoneNumber,
                SpecializationId = requestDto.SpecializationId.Value,
                Bio = requestDto.Bio,
                YearsOfExperience = requestDto.YearsOfExperience.Value,
                HourlyRate = 0, // Will be set by admin
                ApprovalStatus = "Pending",
                IsApproved = false,
                CreatedAt = DateTime.UtcNow
            };

            await _consultantRepository.AddAsync(consultant);
            profileId = consultant.Id;
            approvalStatus = "Pending";

            // Send welcome email with approval notice
            await _emailService.SendEmailAsync(
                user.Email,
                "Welcome to Inova - Application Pending",
                $"<p>Hello {requestDto.FullName},</p><p>Thank you for applying as a consultant. Your application is pending admin approval.</p>"
            );
        }

        // 6. Generate JWT token
        var token = _tokenService.GenerateToken(
            user.Id,
            user.Email,
            requestDto.FullName,
            user.Role,
            profileId
        );

        // 7. Return response
        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = requestDto.FullName,
            Role = user.Role,
            UserId = user.Id,
            ProfileId = profileId,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            Message = requestDto.Role.Equals("Consultant", StringComparison.OrdinalIgnoreCase)
                ? "Registration successful! Your application is pending approval."
                : "Registration successful!",
            ApprovalStatus = approvalStatus
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto requestDto)
    {
        // 1. Validate request
        if (!requestDto.IsValid())
        {
            throw new InvalidOperationException(requestDto.GetValidationErrors());
        }

        // 2. Find user by email
        var user = await _userRepository.GetByEmailAsync(requestDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // 3. Verify password
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(requestDto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // 4. Check if user is active
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is deactivated");
        }

        // 5. Get profile based on role
        int profileId = 0;
        string fullName = string.Empty;
        string approvalStatus = null;

        if (user.Role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
        {
            var customer = await _customerRepository.GetByUserIdAsync(user.Id);
            if (customer != null)
            {
                profileId = customer.Id;
                fullName = customer.FullName;
            }
        }
        else if (user.Role.Equals("Consultant", StringComparison.OrdinalIgnoreCase))
        {
            var consultant = await _consultantRepository.GetByUserIdAsync(user.Id);
            if (consultant != null)
            {
                profileId = consultant.Id;
                fullName = consultant.FullName;
                approvalStatus = consultant.ApprovalStatus;

                // Check if consultant is approved
                if (!consultant.IsApproved)
                {
                    throw new UnauthorizedAccessException("Your consultant application is still pending approval");
                }
            }
        }
        else if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            // Admin doesn't have separate profile
            fullName = "Admin";
            profileId = user.Id;
        }

        // 6. Generate JWT token
        var token = _tokenService.GenerateToken(
            user.Id,
            user.Email,
            fullName,
            user.Role,
            profileId
        );

        // 7. Return response
        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = fullName,
            Role = user.Role,
            UserId = user.Id,
            ProfileId = profileId,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            Message = "Login successful",
            ApprovalStatus = approvalStatus
        };
    }
}