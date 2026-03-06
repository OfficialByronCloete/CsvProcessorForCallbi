import { describe, it } from 'vitest';
import { MyApp } from '../src/my-app';
import { createFixture } from '@aurelia/testing';

describe('my-app', () => {
  it('should render table and pagination controls', async () => {
    const { appHost } = await createFixture(
      '<my-app></my-app>',
      {},
      [MyApp],
    ).started;

    const element = appHost.querySelector('my-app');
    if (element === null) {
      throw new Error('Expected to find my-app element in host');
    }

    const table = element.querySelector('table');
    if (table === null) {
      throw new Error('Expected table to render');
    }

    const uploadInput = element.querySelector('input[type="file"]');
    if (uploadInput === null) {
      throw new Error('Expected upload control to render');
    }

    const previousButton = element.querySelector('.transactions-pagination button');
    if (previousButton === null) {
      throw new Error('Expected pagination controls to render');
    }

    const actionsHeader = element.querySelector('th:last-child');
    if (actionsHeader?.textContent?.trim() !== 'Actions') {
      throw new Error('Expected actions column to render');
    }

  });
});
