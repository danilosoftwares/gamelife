// Permite que o TypeScript reconheça os matchers do jest-dom no expect do Vitest
import '@testing-library/jest-dom';

declare global {
  namespace Vi {
    interface Assertion<T = any> {
      toHaveStyle(style: string | Record<string, string>): void;
    }
  }
}
