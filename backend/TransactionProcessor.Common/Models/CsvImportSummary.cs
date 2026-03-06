namespace TransactionProcessor.Common.Models;

public sealed class CsvImportSummary
{
    public required int AddedCount { get; init; }
    public required int DuplicateCount { get; init; }
    public required int TotalProcessedCount { get; init; }
}
