using Microsoft.EntityFrameworkCore;
using TransactionProcessor.Integrations.DataAccess.Entities;

namespace TransactionProcessor.Integrations.DataAccess.Contexts;

public class TransactionProcessorContext(DbContextOptions<TransactionProcessorContext> options) : DbContext(options)
{
    public DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
    }
}
