using Microsoft.EntityFrameworkCore;
using TransactionProcessor.Common.Models;
using TransactionProcessor.Core.Contracts;
using TransactionProcessor.Integrations.DataAccess.Contexts;
using TransactionProcessor.Integrations.DataAccess.Entities;

namespace TransactionProcessor.Integrations.DataAccess.Repositories
{
    public sealed class TransactionRepository(TransactionProcessorContext dbContext) : ITransactionRepository
    {
        public async Task<int> SubmitTransactionsAsync(IReadOnlyCollection<TransactionModel> transactionsToInsert, CancellationToken cancellationToken)
        {
            if (transactionsToInsert.Count == 0)
                return 0;

            // Remove duplicates from the transactions we want to insert.
            var distinctTransactionsToInsert = transactionsToInsert
                .DistinctBy(t => t.TransactionId)
                .ToList();

            var transactionIdsToInsert = distinctTransactionsToInsert
                .Select(t => t.TransactionId)
                .ToList();

            // Compare and find IDs that already exist in DB.
            var existingDbTransactionIds = await dbContext.Transactions
                .AsNoTracking()
                .Where(t => transactionIdsToInsert.Contains(t.TransactionId))
                .Select(t => t.TransactionId)
                .ToHashSetAsync(cancellationToken);

            // Keep only brand-new transactions.
            var newTransactions = distinctTransactionsToInsert
                .Where(t => !existingDbTransactionIds.Contains(t.TransactionId))
                .Select(t => new TransactionEntity
                {
                    TransactionId = t.TransactionId,
                    TransactionTime = t.TransactionTime,
                    TransactionAmount = t.TransactionAmount,
                    Description = t.Description
                })
                .ToList();

            if (newTransactions.Count == 0)
                return 0;

            await dbContext.Transactions.AddRangeAsync(newTransactions, cancellationToken);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<TransactionModel>> GetTransactionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await dbContext.Transactions.CountAsync(cancellationToken);

            var data = await dbContext.Transactions
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionTime)
                .ThenBy(t => t.TransactionId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionModel
                {
                    TransactionId = t.TransactionId,
                    TransactionTime = t.TransactionTime,
                    TransactionAmount = t.TransactionAmount,
                    Description = t.Description
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<TransactionModel>
            {
                Data = data,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateTransactionAsync(TransactionModel transaction, CancellationToken cancellationToken)
        {
            var existingEntity = await dbContext.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == transaction.TransactionId, cancellationToken);

            if (existingEntity is null)
                return false;

            existingEntity.TransactionTime = transaction.TransactionTime;
            existingEntity.TransactionAmount = transaction.TransactionAmount;
            existingEntity.Description = transaction.Description;

            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var existingEntity = await dbContext.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId, cancellationToken);

            if (existingEntity is null)
                return false;

            dbContext.Transactions.Remove(existingEntity);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
