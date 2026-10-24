
using Microsoft.EntityFrameworkCore;
using NexLedger.Domain.Entities;

namespace NexLedger.Infrastructure.Persistence;

public class NexLedgerDbContext : DbContext
{
    public NexLedgerDbContext(
        DbContextOptions<NexLedgerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<BankTransaction> BankTransactions => Set<BankTransaction>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName)
                .HasMaxLength(150).IsRequired();
            entity.Property(e => e.Email)
                .HasMaxLength(256).IsRequired();
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(30).IsRequired();
            entity.HasIndex(e => e.AccountNumber).IsUnique();

            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3).IsRequired();
            entity.Property(e => e.Balance)
                .HasPrecision(19, 4);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Accounts)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BankTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount)
                .HasPrecision(19, 4);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3).IsRequired();
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(64).IsRequired();
            entity.HasIndex(e => e.ReferenceNumber).IsUnique();
            entity.Property(e => e.Status)
                .HasMaxLength(30).IsRequired();

            entity.HasOne(e => e.SourceAccount)
                .WithMany()
                .HasForeignKey(e => e.SourceAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DestinationAccount)
                .WithMany()
                .HasForeignKey(e => e.DestinationAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Transaction_PositiveAmount",
                    "[Amount] > 0");
                t.HasCheckConstraint(
                    "CK_Transaction_DifferentAccounts",
                    "[SourceAccountId] <> [DestinationAccountId]");
            });
        });

        modelBuilder.Entity<LedgerEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount)
                .HasPrecision(19, 4);
            entity.Property(e => e.EntryType)
                .HasMaxLength(10).IsRequired();

            entity.HasOne(e => e.Transaction)
                .WithMany(t => t.LedgerEntries)
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_LedgerEntry_PositiveAmount",
                    "[Amount] > 0");
                t.HasCheckConstraint(
                    "CK_LedgerEntry_Type",
                    "[EntryType] IN ('Debit', 'Credit')");
            });
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action)
                .HasMaxLength(100).IsRequired();
            entity.Property(e => e.EntityName)
                .HasMaxLength(100).IsRequired();
            entity.Property(e => e.EntityId)
                .HasMaxLength(100).IsRequired();
            entity.Property(e => e.PerformedBy)
                .HasMaxLength(256);
        });
    }
}
