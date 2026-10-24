
namespace NexLedger.Domain.Entities;

public class BankTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SourceAccountId { get; set; }
    public Guid DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "LKR";
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public BankAccount SourceAccount { get; set; } = null!;
    public BankAccount DestinationAccount { get; set; } = null!;

    public ICollection<LedgerEntry> LedgerEntries { get; set; }
        = new List<LedgerEntry>();
}
