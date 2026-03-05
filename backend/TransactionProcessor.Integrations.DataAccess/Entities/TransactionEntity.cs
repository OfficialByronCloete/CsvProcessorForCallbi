using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionProcessor.Integrations.DataAccess.Entities;

[Table("Transactions")]
public class TransactionEntity
{
    public Guid TransactionId { get; set; }
    public DateTime TransactionTime { get; set; }
    public decimal TransactionAmount { get; set; }
    public required string Description { get; set; }
}

public sealed class TransactionEntityConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.HasKey(t => t.TransactionId);

        builder.Property(t => t.TransactionId)
            .HasColumnType("TEXT");

        builder.Property(t => t.TransactionTime)
            .IsRequired()
            .HasColumnType("TEXT");

        builder.Property(t => t.TransactionAmount)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("TEXT");

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(t => t.TransactionTime);
    }
}