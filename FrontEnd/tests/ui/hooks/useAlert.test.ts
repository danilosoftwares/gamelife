import { act } from 'react-dom/test-utils';
import { renderHook } from '@testing-library/react';
import { useAlertStore } from '../../../src/ui/hooks/useAlert';
import { describe, expect, it, vi } from 'vitest';

describe('useAlertStore', () => {
  it('deve exibir e limpar mensagem de alerta', () => {
    const { result } = renderHook(() => useAlertStore());
    act(() => {
      result.current.showAlert('Teste', 'success');
    });
    expect(result.current.message).toBe('Teste');
    expect(result.current.type).toBe('success');
    act(() => {
      vi.useFakeTimers();
    });
    expect(result.current.message).toBe("Teste");
  });
});
