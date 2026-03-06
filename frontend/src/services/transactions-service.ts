import { environment } from '../environments/environment';
import type { PagedResult } from '../models/paged-result';
import type { ProblemDetails } from '../models/problem-details';
import type { Transaction } from '../models/transaction';

const transactionsEndpoint = `${environment.apiBaseUrl}/api/transactions`;

export class TransactionsService {
  public async getTransactions(pageNumber: number, pageSize: number): Promise<PagedResult<Transaction>> {
    const response = await fetch(`${transactionsEndpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    if (!response.ok) {
      throw new Error(await this.getErrorMessage(response, `Failed to load transactions (${response.status})`));
    }

    return await response.json() as PagedResult<Transaction>;
  }

  public async uploadTransactionsCsv(file: File): Promise<string> {
    const formData = new FormData();
    formData.append('file', file);
    const response = await fetch(`${transactionsEndpoint}/imports`, {
      method: 'POST',
      body: formData,
    });
    if (!response.ok) {
      throw new Error(await this.getErrorMessage(response, `Failed to upload CSV (${response.status})`));
    }

    const result = await response.json() as { message?: string };
    return result.message ?? 'CSV uploaded successfully.';
  }

  public async deleteTransaction(transactionId: string): Promise<void> {
    const response = await fetch(`${transactionsEndpoint}/${transactionId}`, {
      method: 'DELETE',
    });
    if (!response.ok) {
      throw new Error(await this.getErrorMessage(response, `Failed to delete transaction (${response.status})`));
    }
  }

  public async updateTransaction(transaction: Transaction): Promise<void> {
    const response = await fetch(`${transactionsEndpoint}/${transaction.transactionId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(transaction),
    });
    if (!response.ok) {
      throw new Error(await this.getErrorMessage(response, `Failed to update transaction (${response.status})`));
    }
  }

  private async getErrorMessage(response: Response, fallbackMessage: string): Promise<string> {
    const contentType = response.headers.get('content-type') ?? '';
    if (!contentType.toLowerCase().includes('json')) {
      const responseText = await response.text();
      return responseText.trim() || fallbackMessage;
    }

    const problem = await response.json() as ProblemDetails;
    return problem.detail ?? problem.title ?? fallbackMessage;
  }
}
