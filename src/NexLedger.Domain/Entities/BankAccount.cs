
namespace NexLedger.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = "LKR";
    public decimal Balance { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Customer Customer { get; set; } = null!;
}
