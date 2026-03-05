using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionProcessor.Common.Models;

public sealed class PagedResult<T>
{
    public required IReadOnlyList<T> Data { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required int TotalCount { get; init; }
}
