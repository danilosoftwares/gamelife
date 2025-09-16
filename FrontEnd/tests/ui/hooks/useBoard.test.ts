import { renderHook, act } from '@testing-library/react';
import { useBoardStore } from '../../../src/ui/hooks/useBoard';
import { describe, expect, it } from 'vitest';

describe('useBoardStore', () => {
  it('deve inicializar o board corretamente', () => {
    const { result } = renderHook(() => useBoardStore());
    expect(result.current.board.length).toBeGreaterThan(0);
  });

  it('deve alternar célula', () => {
    const { result } = renderHook(() => useBoardStore());
    act(() => {
      result.current.toggleCell(0, 0);
    });
    expect(typeof result.current.board[0][0]).toBe('boolean');
  });
});
