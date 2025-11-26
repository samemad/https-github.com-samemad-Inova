namespace Inova.Application.DTOs.Session;

public class BookSessionRequestDto
{
    public int ConsultantId { get; set; }  
    public DateTime ScheduledDate { get; set; } 
    public TimeSpan ScheduledTime { get; set; } 
    public decimal  DurationHours { get; set; }  
}