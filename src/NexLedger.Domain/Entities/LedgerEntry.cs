
namespace NexLedger.Domain.Entities;

public class LedgerEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TransactionId { get; set; }
    public Guid AccountId { get; set; }
    public string EntryType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public BankTransaction Transaction { get; set; } = null!;
    public BankAccount Account { get; set; } = null!;
}
