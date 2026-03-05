using TransactionProcessor.Common.Models;

namespace TransactionProcessor.Core.Contracts;

public interface ITransactionService
{
    Task ParseAndSubmitTransactionCsvAsync(Stream csvStream, CancellationToken cancellationToken);
    Task<PagedResult<TransactionModel>> GetTransactionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> UpdateTransactionAsync(TransactionModel transaction, CancellationToken cancellationToken);
    Task<bool> DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken);
}
