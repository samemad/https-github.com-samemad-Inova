public class PaymentResponseDto
{
    public int Id { get; set; }  
    public int SessionId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } 
    public DateTime CreatedAt { get; set; }  
}