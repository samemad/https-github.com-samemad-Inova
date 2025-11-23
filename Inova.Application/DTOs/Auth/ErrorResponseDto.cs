namespace Inova.Application.DTOs.Auth;

public class ErrorResponseDto
{
    public string Message { get; set; }
    public string Details { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; }

    public ErrorResponseDto()
    {
        Timestamp = DateTime.UtcNow;
    }

    public ErrorResponseDto(string message, int statusCode) : this()
    {
        Message = message;
        StatusCode = statusCode;
    }

    public ErrorResponseDto(string message, string details, int statusCode) : this()
    {
        Message = message;
        Details = details;
        StatusCode = statusCode;
    }
}