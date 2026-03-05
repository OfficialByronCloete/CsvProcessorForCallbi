namespace TransactionProcessor.Common.Models;

public class TransactionModel
{
    public Guid TransactionId { get; set; }
    public DateTime TransactionTime { get; set; }
    public decimal TransactionAmount { get; set; }
    public required string Description { get; set; }
}
