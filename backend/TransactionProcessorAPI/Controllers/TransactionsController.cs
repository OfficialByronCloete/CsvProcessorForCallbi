using Microsoft.AspNetCore.Mvc;
using TransactionProcessor.Common.Models;
using TransactionProcessor.Core.Contracts;

namespace TransactionProcessorAPI.WebAPI.Controllers;

[ApiController]
[Route("api/transactions")]
public sealed class TransactionsController(
    ITransactionService transactionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var pagedTransactions = await transactionService.GetTransactionsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(pagedTransactions);
    }

    [HttpPost("imports")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCsv(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file provided.");

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Invalid file type. Only CSV files are allowed.");

        await using var stream = file.OpenReadStream();
        var importSummary = await transactionService.ParseAndSubmitTransactionCsvAsync(stream, cancellationToken);
        var message = importSummary.AddedCount == 0
            ? $"No new transactions were added. Duplicates found: {importSummary.DuplicateCount}."
            : $"Upload complete. Added {importSummary.AddedCount} transaction(s). Duplicates found: {importSummary.DuplicateCount}.";
        return Ok(new { message, importSummary.AddedCount, importSummary.DuplicateCount, importSummary.TotalProcessedCount });
    }

    [HttpPut("{transactionId:guid}")]
    public async Task<IActionResult> UpdateTransaction(Guid transactionId, [FromBody] TransactionModel transaction, CancellationToken cancellationToken)
    {
        if (transactionId != transaction.TransactionId)
            return BadRequest("Route transactionId must match body transactionId.");

        var updated = await transactionService.UpdateTransactionAsync(transaction, cancellationToken);

        if (!updated)
            return NotFound($"Transaction '{transactionId}' was not found.");

        return NoContent();
    }

    [HttpDelete("{transactionId:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid transactionId, CancellationToken cancellationToken)
    {
        var deleted = await transactionService.DeleteTransactionAsync(transactionId, cancellationToken);

        if (!deleted)
            return NotFound($"Transaction '{transactionId}' was not found.");

        return NoContent();
    }
}
