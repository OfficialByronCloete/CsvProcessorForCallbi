import { mkdirSync, writeFileSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { randomUUID } from 'node:crypto';

const defaultCount = 1000;
const args = process.argv.slice(2);

const countArg = args.find((arg) => arg.startsWith('--count='));
const outputArg = args.find((arg) => arg.startsWith('--output='));
const delimiterArg = args.find((arg) => arg.startsWith('--delimiter='));
const includeTimeArg = args.find((arg) => arg.startsWith('--include-time='));
const positionalCountArg = args.find((arg) => /^\d+$/.test(arg));

const parsedCount = Number.parseInt(
  countArg?.split('=')[1] ?? positionalCountArg ?? `${defaultCount}`,
  10,
);

if (!Number.isInteger(parsedCount) || parsedCount <= 0) {
  throw new Error('Count must be a positive integer.');
}

const outputPath = resolve(
  process.cwd(),
  outputArg?.split('=')[1] ?? 'sample-transactions-1000.csv',
);
const parsedDelimiter = delimiterArg?.split('=')[1] ?? ',';
const delimiter = parsedDelimiter === '\\t' || parsedDelimiter.toLowerCase() === 'tab'
  ? '\t'
  : parsedDelimiter;
const includeTime = includeTimeArg?.split('=')[1]?.toLowerCase() !== 'false';

if (delimiter.length === 0) {
  throw new Error('Delimiter cannot be empty.');
}

const formatDate = (date) => {
  const day = `${date.getUTCDate()}`.padStart(2, '0');
  const month = `${date.getUTCMonth() + 1}`.padStart(2, '0');
  const year = `${date.getUTCFullYear()}`;
  if (!includeTime) {
    return `${day}/${month}/${year}`;
  }

  const hour = `${date.getUTCHours()}`.padStart(2, '0');
  const minute = `${date.getUTCMinutes()}`.padStart(2, '0');
  const second = `${date.getUTCSeconds()}`.padStart(2, '0');
  return `${day}/${month}/${year} ${hour}:${minute}:${second}`;
};

const lines = [`TransactionTime${delimiter}Amount${delimiter}Description${delimiter}TransactionId`];
const startDate = Date.UTC(2026, 0, 1);

for (let index = 1; index <= parsedCount; index++) {
  const randomSeconds = Math.floor(Math.random() * 24 * 60 * 60);
  const transactionDate = new Date(startDate + (index - 1) * 24 * 60 * 60 * 1000 + randomSeconds * 1000);
  const transactionTime = formatDate(transactionDate);
  const amount = ((((index * 137) % 500000) + 100) / 100).toFixed(2);
  const description = `Generated transaction ${index}`;
  const transactionId = randomUUID();

  lines.push(`${transactionTime}${delimiter}${amount}${delimiter}${description}${delimiter}${transactionId}`);
}

mkdirSync(dirname(outputPath), { recursive: true });
writeFileSync(outputPath, `${lines.join('\n')}\n`, 'utf8');

console.log(`Created ${parsedCount} records at ${outputPath} using delimiter "${delimiter === '\t' ? '\\t' : delimiter}" (include-time=${includeTime})`);
