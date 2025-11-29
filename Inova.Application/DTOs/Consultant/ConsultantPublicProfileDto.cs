namespace Inova.Application.DTOs.Consultant;

public class ConsultantPublicProfileDto  // ← Must be PUBLIC!
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string SpecializationName { get; set; }
    public string Bio { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal HourlyRate { get; set; }
    public string ProfileImageUrl { get; set; }
    public string CoverImageUrl { get; set; }
    public int TotalSessions { get; set; }
    public double Rating { get; set; }
}