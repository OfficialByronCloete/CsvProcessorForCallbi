using TransactionProcessor.Common.Models;

namespace TransactionProcessor.Core.Contracts;

public interface ITransactionRepository
{
    Task<int> SubmitTransactionsAsync(IReadOnlyCollection<TransactionModel> transactions, CancellationToken cancellationToken);
    Task<PagedResult<TransactionModel>> GetTransactionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> UpdateTransactionAsync(TransactionModel transaction, CancellationToken cancellationToken);
    Task<bool> DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken);
}
